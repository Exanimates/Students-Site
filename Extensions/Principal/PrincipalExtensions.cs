using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extensions.Principal
{
    public static class PrincipalExtensions
    {
        public static bool IsDean(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole("1");
        }

        public static bool IsStudent(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole("2");
        }

        public static bool IsTeacher(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole("3");
        }
    }
}
