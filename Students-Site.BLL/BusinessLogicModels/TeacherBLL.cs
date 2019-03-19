using System.Collections.Generic;

namespace Students_Site.BLL.BusinessLogicModels
{
    public class TeacherBLL
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        public int UserId { get; set; }
        public UserBLL User { get; set; }

        public IEnumerable<StudentBLL> Students;
    }
}