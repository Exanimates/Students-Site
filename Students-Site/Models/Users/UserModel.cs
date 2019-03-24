using Students_Site.Models.Home;
using System.ComponentModel.DataAnnotations;

namespace Students_Site.Models.Users
{
    public class UserModel : IndexModel
    {
        public int UserId { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public int RoleId { get; set; }

        public string RoleName { get; set; }
    }
}