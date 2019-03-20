using System.Collections.Generic;
using Students_Site.Models.Students;
using Students_Site.Models.Users;

namespace Students_Site.Models.Teachers
{
    public class TeacherModel : UserModel
    {
        public int Id { get; set; }

        public int SubjectId;

        public bool IsSelected { get; set; }

        public IEnumerable<StudentModel> Students;

        public string SubjectName { get; set; }
    }
}