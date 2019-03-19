﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Students_Site.Models
{
    public class UserModel
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
    }
}