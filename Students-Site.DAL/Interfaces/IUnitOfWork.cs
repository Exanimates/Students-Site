using Students_Site.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Students_Site.DAL.EF;
using Students_Site.DAL.Repositories;

namespace Students_Site.DAL.Interfaces
{
    public interface IUnitOfWork: IDisposable
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

        public IRepository<Student> StudentRepository
        {
            get
            {

                if (_studentRepository == null)
                {
                    _studentRepository = new StudentRepository(_dbContext);
                }

                return _studentRepository;
            }
        }

        public IRepository<StudentTeacher> StudentTeacherRepository
        {
            get
            {

                if (_studentRepository == null)
                {
                    _studentTeachers = new StudentTeacherRepository(_dbContext);
                }

                return _studentTeachers;
            }
        }

        public IRepository<User> UserRepository
        {
            get
            {

                if (_studentRepository == null)
                {
                    _userRepository = new UserRepository(_dbContext);
                }

                return _userRepository;
            }
        }

        public IRepository<Subject> SubjectRepository
        {
            get
            {

                if (_studentRepository == null)
                {
                    _subjectsRepository = new SubjectRepository(_dbContext);
                }

                return _subjectsRepository;
            }
        }

        public IRepository<Role> RoleRepository
        {
            get
            {

                if (_studentRepository == null)
                {
                    _roleRepository = new RoleRepository(_dbContext);
                }

                return _roleRepository;
            }
        }

        public IRepository<Teacher> TeacherRepository
        {
            get
            {

                if (_studentRepository == null)
                {
                    _teacherRepository = new TeacherRepository(_dbContext);
                }

                return _teacherRepository;
            }
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
