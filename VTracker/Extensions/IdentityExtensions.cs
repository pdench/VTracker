using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace VTracker.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetFirstName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("FirstName");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
        public static int GetNumLogins(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("NumLogins");
            if (claim == null)
            {
                return 0;
            }
            else
            {
                int numLogins = Int32.Parse(claim.Value);
                return numLogins;
            }
        }
    }
}