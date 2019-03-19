using System;
using System.Collections.Generic;
using System.Text;

namespace Students_Site.BLL.Business_Logic_Models
{
    public class UserBLL
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
    }
}
