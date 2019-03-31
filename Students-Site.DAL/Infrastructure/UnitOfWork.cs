using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Students_Site.DAL.EF;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Repositories;

namespace Students_Site.DAL.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> UserRepository { get; }
        IRepository<Teacher> TeacherRepository { get; }
        IRepository<Subject> SubjectRepository { get; }
        StudentTeacherRepository StudentTeacherRepository { get; }
        IRepository<Student> StudentRepository { get; }
        void Save();
    }

    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationContext _dbContext;

        private IRepository<User> _userRepository;
        private IRepository<Teacher> _teacherRepository;
        private IRepository<Subject> _subjectsRepository;
        private IRepository<Student> _studentRepository;
        private StudentTeacherRepository _studentTeachers;

        private bool _disposed = false;


        public UnitOfWork(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<Student> StudentRepository => _studentRepository ?? (_studentRepository = new StudentRepository(_dbContext));

        public StudentTeacherRepository StudentTeacherRepository => 
            _studentTeachers ?? (_studentTeachers = new StudentTeacherRepository(_dbContext));

        public IRepository<User> UserRepository => _userRepository ?? (_userRepository = new UserRepository(_dbContext));

        public IRepository<Subject> SubjectRepository => _subjectsRepository ?? (_subjectsRepository = new SubjectRepository(_dbContext));

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