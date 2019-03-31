using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.Repositories
{
    public class StudentRepository : RepositoryBase<Student>
    {
        public StudentRepository(ApplicationContext context) : base(context)
        {
        }
    }
}