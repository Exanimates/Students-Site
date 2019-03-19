using System.Collections.Generic;

namespace Students_Site.DAL.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Teacher> Teachers { get; set; }
    }
}