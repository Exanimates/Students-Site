using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Interfaces;

namespace Students_Site.DAL.Repositories
{
    public class UserRepository : GenericRepository<User>
    {
        public UserRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
