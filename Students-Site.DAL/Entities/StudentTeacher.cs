using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Students_Site.DAL.Entities
{
    public class StudentTeacher
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        public float Grade { get; set; }
    }
}