﻿using System.Collections.Generic;
using Students_Site.Models.Teachers;
using Students_Site.Models.Users;

namespace Students_Site.Models.Students
{
    public class StudentMakeModel : UserModel
    {
        public List<TeacherModel> TeachersList { get; set; }
    }
}
