using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Students_Site.DAL.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>
    {
        public SubjectRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
