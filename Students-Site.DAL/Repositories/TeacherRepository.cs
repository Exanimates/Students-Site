using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.Repositories
{
    public class TeacherRepository : RepositoryBase<Teacher>
    {
        public TeacherRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
