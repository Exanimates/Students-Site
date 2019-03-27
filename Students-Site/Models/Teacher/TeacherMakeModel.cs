using System.Collections.Generic;
using Students_Site.Models.Student;
using Students_Site.Models.Subject;
using Students_Site.Models.Users;

namespace Students_Site.Models.Teacher
{
    public class TeacherMakeModel : UserModel
    {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public IList<StudentModel> Students { get; set; }
        public IList<SubjectModel> Subjects { get; set; }
    }
}
