using System.Collections.Generic;
using Students_Site.Models.Students;
using Students_Site.Models.Subjects;
using Students_Site.Models.Users;

namespace Students_Site.Models.Teachers
{
    public class TeacherMakeModel : UserModel
    {
        public int SubjectId { get; set; }
        public List<StudentModel> Students { get; set; }
        public List<SubjectModel> Subjects { get; set; }
    }
}
