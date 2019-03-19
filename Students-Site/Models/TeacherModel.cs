using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Students_Site.Models
{
    public class TeacherModel : UserModel
    {
        public int SubjectId;

        public IEnumerable<StudentModel> Students;

        [Required]
        public string SubjectName;
    }
}