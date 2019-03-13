using System;
using System.Collections.Generic;
using System.Text;

namespace Students_Site.DAL.Entities
{
    class Teacher
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }

        public Subject Subject { get; set; }
    }
}
