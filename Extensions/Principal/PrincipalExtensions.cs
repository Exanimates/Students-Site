using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;
using Students_Site.DAL.Enums;

namespace Extensions.Principal
{
    public static class PrincipalExtensions
    {
        public static bool IsDean(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole($"{Roles.Dean}");
        }

        public static bool IsStudent(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole($"{Roles.Student}");
        }

        public static bool IsTeacher(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole($"{Roles.Teacher}");
        }
    }
}
