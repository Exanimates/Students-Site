using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.Repositories
{
    public class StudentTeacherRepository : RepositoryBase<StudentTeacher>
    {
        public StudentTeacherRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
