using System.ComponentModel.DataAnnotations;

namespace Students_Site.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public Student  Student { get; set; }
        public Teacher Teacher { get; set; }
    }
}