using System.Collections.Generic;
using Students_Site.Models.Teacher;
using Students_Site.Models.Users;

namespace Students_Site.Models.Student
{
    public class StudentMakeModel : UserModel
    {
        public int Id { get; set; }
        public List<TeacherModel> TeachersList { get; set; }
    }
}
