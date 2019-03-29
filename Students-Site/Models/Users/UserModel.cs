using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Students_Site.DAL.Enums;

namespace Students_Site.Models.Users
{
    public class UserModel
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

        public Roles Role { get; set; }

        public string RoleName { get; set; }
    }
}