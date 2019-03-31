using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Students_Site.Models.Teacher;
using Students_Site.Models.Users;

namespace Students_Site.Models.Student
{
    public class StudentModel : UserModel
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public double AveradeScore { get; set; }
        public IEnumerable<TeacherModel> Teachers { get; set; }
    }
}