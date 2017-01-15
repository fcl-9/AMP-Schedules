/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using Microsoft.Identity.Client;

namespace AMPSchedules.TokenStorage
{

    // Store the user's token information.
    public class UserTokenCache : TokenCache
    {
        private static readonly object FileLock = new object();
        private readonly string mUserUniqueId;

        private UserTokenCacheDb db = new UserTokenCacheDb();
        private UserTokenCacheEntry mCacheEntry;

        public UserTokenCache( string aUserUniqueId )
        {
            mUserUniqueId = aUserUniqueId;

            AfterAccess  = AfterAccessNotification;
            BeforeAccess = BeforeAccessNotification;
            BeforeWrite  = BeforeWriteNotification;

            Load();
        }

        public void Load()
        {
            lock ( FileLock )
            {
                if ( mCacheEntry == null )
                {
                    mCacheEntry = db.TokenCaches.FirstOrDefault( c => c.UserUniqueId == mUserUniqueId );
                }
                else
                {
                    // Retrieve last write from the DB
                    var status = from e in db.TokenCaches
                                 where ( e.UserUniqueId == mUserUniqueId )
                                 select new { LastWrite = e.LastWrite };

                    // If the in-memory copy is older than the persistent copy
                    if ( status.First().LastWrite > mCacheEntry.LastWrite )
                    {
                        // Read from from storage, update in-memory copy
                        mCacheEntry = db.TokenCaches.FirstOrDefault( c => c.UserUniqueId == mUserUniqueId );
                    }
                }
                if ( mCacheEntry != null )
                {
                    this.Deserialize( MachineKey.Unprotect( mCacheEntry.CacheBits, "MSALCache" ) );
                }
            }
        }

        public void Persist()
        {
            if ( ! HasStateChanged ) return;

            lock (FileLock)
            {
                mCacheEntry = new UserTokenCacheEntry()
                {
                    UserUniqueId = mUserUniqueId,
                    CacheBits = MachineKey.Protect( Serialize(), "MSALCache" ),
                    LastWrite = DateTime.Now
                };

                db.Entry( mCacheEntry ).State = EntityState.Added;
                db.SaveChanges();

                // After the write operation takes place, restore the HasStateChanged bit to false.
                HasStateChanged = false;
            }
        }

        // Empties the persistent store.
        public override void Clear( string aClientId )
        {
            base.Clear(aClientId);

            var cache = db.TokenCaches.FirstOrDefault( c => c.UserUniqueId == mUserUniqueId );
            if ( cache != null )
            {
                db.TokenCaches.Remove( cache );
                db.SaveChanges();
            }
        }

        // Triggered right before ADAL needs to access the cache.
        // Reload the cache from the persistent store in case it changed since the last access.
        private void BeforeAccessNotification( TokenCacheNotificationArgs args )
        {
            Load();
        }

        // Triggered right after ADAL accessed the cache.
        private void AfterAccessNotification( TokenCacheNotificationArgs args )
        {
            // If the access operation resulted in a cache update
            if ( ! HasStateChanged ) return;

            Persist();
        }

        private void BeforeWriteNotification(TokenCacheNotificationArgs args)
        {
        }
    }
}