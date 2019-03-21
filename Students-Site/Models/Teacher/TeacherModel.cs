﻿using System.Collections.Generic;
using Students_Site.Models.Student;
using Students_Site.Models.Users;

namespace Students_Site.Models.Teacher
{
    public class TeacherModel : UserModel
    {
        public int Id { get; set; }

        public int SubjectId;

        public bool IsSelected { get; set; }

        public IEnumerable<StudentModel> Students;

        public string SubjectName { get; set; }
    }
}