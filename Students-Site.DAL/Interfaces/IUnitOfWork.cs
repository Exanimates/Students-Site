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
}
