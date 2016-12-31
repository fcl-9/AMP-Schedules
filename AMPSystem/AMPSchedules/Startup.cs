/* 
*  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. 
*  See LICENSE in the source repository root for complete license information. 
*/

using Microsoft.Owin;
using Owin;
using Startup = AMPSchedules.Startup;

[assembly: OwinStartup(typeof(Startup))]

namespace AMPSchedules
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}