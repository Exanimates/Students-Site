using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Students_Site.DAL.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}