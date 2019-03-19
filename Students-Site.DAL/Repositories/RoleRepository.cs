using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.Repositories
{
    public class RoleRepository : BaseRepository<Role>
    {
        public RoleRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
