using System;
using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Repositories;

namespace Students_Site.DAL.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Teacher> TeacherRepository { get; }
        IRepository<Role> RoleRepository { get; }
        IRepository<Subject> SubjectRepository { get; }
        IRepository<StudentTeacher> StudentTeacherRepository { get; }
        IRepository<Student> StudentRepository { get; }
        void Save();
    }

    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationContext _dbContext;

        IRepository<User> _userRepository;
        IRepository<Teacher> _teacherRepository;
        IRepository<Role> _roleRepository;
        IRepository<Subject> _subjectsRepository;
        IRepository<Student> _studentRepository;
        IRepository<StudentTeacher> _studentTeachers;

        private bool _disposed = false;


        public UnitOfWork(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<Student> StudentRepository => _studentRepository ?? (_studentRepository = new StudentRepository(_dbContext));

        public IRepository<StudentTeacher> StudentTeacherRepository => _studentTeachers ?? (_studentTeachers = new StudentTeacherRepository(_dbContext));

        public IRepository<User> UserRepository => _userRepository ?? (_userRepository = new UserRepository(_dbContext));

        public IRepository<Subject> SubjectRepository => _subjectsRepository ?? (_subjectsRepository = new SubjectRepository(_dbContext));

        public IRepository<Role> RoleRepository => _roleRepository ?? (_roleRepository = new RoleRepository(_dbContext));

        public IRepository<Teacher> TeacherRepository => _teacherRepository ?? (_teacherRepository = new TeacherRepository(_dbContext));

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
                _dbContext.Dispose();

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
