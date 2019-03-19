using System.Collections.Generic;

namespace Students_Site.DAL.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<User> Users { get; set; }
    }
}