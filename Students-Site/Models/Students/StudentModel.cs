using System.Collections.Generic;
using Students_Site.Models.Teachers;
using Students_Site.Models.Users;

namespace Students_Site.Models.Students
{
    public class StudentModel : UserModel
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public IEnumerable<TeacherModel> Teachers;
    }
}