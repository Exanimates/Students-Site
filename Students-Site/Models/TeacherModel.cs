using System.ComponentModel.DataAnnotations;

namespace Students_Site.Models
{
    public class TeacherModel : UserModel
    {
        public int SubjectId;

        [Required]
        public string SubjectName;
    }
}