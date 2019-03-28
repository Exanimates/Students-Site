using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;

namespace Students_Site.DAL.Repositories
{
    public class StudentTeacherRepository : RepositoryBase<StudentTeacher>
    {
        public StudentTeacherRepository(ApplicationContext context) : base(context)
        {
        }

        public StudentTeacher Get(int studentId, int teacherId)
        {
            return _dbSet.Find(studentId, teacherId);
        }

        public void Delete(int studentId, int teacherId)
        {
            var entityToDelete = _dbSet.Find(studentId, teacherId);
            if (entityToDelete != null)
                _dbSet.Remove(entityToDelete);
        }
    }
}