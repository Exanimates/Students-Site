﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Students_Site.DAL.Entities
{
    public class Teacher
    {
        public int TeacherId { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public int SubjectId { get; set; }

        public Subject Subject { get; set; }

        public IList<StudentTeacher> StudentTeachers { get; set; }

        public Teacher()
        {
            StudentTeachers = new List<StudentTeacher>();
        }
    }
}
