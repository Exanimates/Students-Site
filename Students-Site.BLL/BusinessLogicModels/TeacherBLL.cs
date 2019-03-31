using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Students_Site.BLL.BusinessLogicModels
{
    public class TeacherBLL
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        public float Grade { get; set; }

        public int UserId { get; set; }
        public UserBLL User { get; set; }

        public IEnumerable<StudentBLL> Students;
    }
}