using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Website.HangFireOptions
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            if (ClaimUtility.ClaimUtility.GetRoles(httpContext.User).Where(p => p == "Admin").Count() >= 1)
                return httpContext.User.Identity.IsAuthenticated;
            return false;
        }
    }
}
