using System.Collections.Generic;

namespace Students_Site.DAL.Entities
{
    public class Student
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }

        public User User { get; set; }

        public IList<StudentTeacher> StudentTeachers { get; set; }

        public Student()
        {
            StudentTeachers = new List<StudentTeacher>();
        }
    }
}