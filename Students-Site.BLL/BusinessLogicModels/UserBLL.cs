using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Students_Site.DAL.Enums;

namespace Students_Site.BLL.BusinessLogicModels
{
    public class UserBLL
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Password { get; set; }
        public string Salt { get; set; }

        public Roles Role { get; set; }
    }
}