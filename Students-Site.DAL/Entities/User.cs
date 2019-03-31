using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Students_Site.DAL.Enums;

namespace Students_Site.DAL.Entities
{
    public class User : EntityBase
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Salt { get; set; }
        [Required]
        public string Password { get; set; }

        public Roles Role { get; set; }

        public Student  Student { get; set; }
        public Teacher Teacher { get; set; }
    }
}