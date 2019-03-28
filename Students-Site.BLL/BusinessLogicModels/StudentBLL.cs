using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Students_Site.BLL.BusinessLogicModels
{
    public class StudentBLL
    {
        public int Id { get; set; }
        public double AverageScore { get; set; }

        public int UserId { get; set; }
        public UserBLL User { get; set; }

        public IEnumerable<TeacherBLL> Teachers;
    }
}