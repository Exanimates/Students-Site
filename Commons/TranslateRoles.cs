using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class TranslateRoles
    {
        public static readonly Dictionary<string, string> TranslateRoleDictionary
            = new Dictionary<string, string>
            {
                { "Dean", "Декан" },
                { "Teacher", "Учитель" },
                { "Student", "Студент" }
            };
    }
}
