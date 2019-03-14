using System;
using System.Collections.Generic;
using System.Text;

namespace Students_Site.DAL.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }

        public List<StudentTeacher> StudentTeachers { get; set; }

        public Student()
        {
            StudentTeachers = new List<StudentTeacher>();
        }
    }
}
