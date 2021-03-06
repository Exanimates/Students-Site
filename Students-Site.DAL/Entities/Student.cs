﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Students_Site.DAL.Entities
{
    public class Student : EntityBase
    {        
        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<StudentTeacher> StudentTeachers { get; set; }

        public Student()
        {
            StudentTeachers = new List<StudentTeacher>();
        }
    }
}