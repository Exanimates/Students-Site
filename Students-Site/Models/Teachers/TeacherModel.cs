using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Students_Site.Models.Students;
using Students_Site.Models.Users;

namespace Students_Site.Models.Teachers
{
    public class TeacherModel : UserModel
    {
        public int Id;

        public int SubjectId;

        public IEnumerable<StudentModel> Students;

        [Required]
        public string SubjectName;
    }
}