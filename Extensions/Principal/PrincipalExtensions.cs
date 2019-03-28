using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Claims;

namespace Extensions.Principal
{
    public static class PrincipalExtensions
    {
        public static bool IsDean(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole("Декан");
        }

        public static bool IsStudent(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole("Студент");
        }

        public static bool IsTeacher(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.IsInRole("Учитель");
        }
    }
}
