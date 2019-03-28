using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Students_Site.DAL.Entities
{
    public class Role : EntityBase
    {
        [Required]
        public string Name { get; set; }
        
        public ICollection<User> Users { get; set; }
    }
}