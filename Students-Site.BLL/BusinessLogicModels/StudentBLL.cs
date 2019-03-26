using System.Collections.Generic;
using Students_Site.DAL.Entities;

namespace Students_Site.BLL.BusinessLogicModels
{
    public class StudentBLL
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsSelected { get; set; }
        public double AverageScore { get; set; }

        public UserBLL User { get; set; }

        public IEnumerable<TeacherBLL> Teachers;
    }
}