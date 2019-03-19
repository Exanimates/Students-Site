using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.Repositories
{
    public class SubjectRepository : RepositoryBase<Subject>
    {
        public SubjectRepository(ApplicationContext context) : base(context)
        {
        }
    }
}