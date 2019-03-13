using System;
using System.Collections.Generic;
using System.Text;

namespace Students_Site.DAL.Entities
{
    class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<User> Users { get; set; }
    }
}
