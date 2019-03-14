using System;
using System.Collections.Generic;
using System.Text;

namespace Students_Site.DAL.Entities
{
    class Student
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
