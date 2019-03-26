﻿using System.ComponentModel.DataAnnotations;
using Students_Site.Models.Home;

namespace Students_Site.Models.Account
{
    public class LoginModel : IndexModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
