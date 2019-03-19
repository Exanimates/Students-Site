using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.Repositories
{
    public class UserRepository : RepositoryBase<User>
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }
    }
}