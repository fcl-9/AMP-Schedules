using System.Data.Entity;

namespace AMPSchedules.TokenStorage
{
    public class UserTokenCacheDb : DbContext
    {
        public UserTokenCacheDb() : base( "name=TokensCacheConnection" )
        {
        }

        public virtual DbSet<UserTokenCacheEntry> TokenCaches { get; set; }
    }
}