using System.Collections.Generic;

namespace Students_Site.Models
{
    public class StudentModel : UserModel
    {
        public IEnumerable<TeacherModel> Teachers;
    }
}