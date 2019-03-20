using System.Collections.Generic;
using Students_Site.Models.Teachers;
using Students_Site.Models.Users;

namespace Students_Site.Models.Students
{
    public class StudentModel : UserModel
    {
        public IEnumerable<TeacherModel> Teachers;
    }
}