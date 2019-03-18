using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Students_Site.BLL.Business_Logic_Models;
using Students_Site.BLL.Helpers;
using Students_Site.BLL.Interfaces;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Interfaces;

namespace Students_Site.BLL.Services
{
    public class StudentService : IStudentService
    {
        IUnitOfWork Database { get; set; }

        public StudentService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public void MakeStudent(UserBLL userBll, IEnumerable<int> teachersId)
        {
            var user = new User
            {
                FirstName = userBll.FirstName,
                LastName = userBll.LastName,
                Login = userBll.Login,
                Password = userBll.Password,
                RoleId = userBll.RoleId
            };

            Database.UserRepository.Create(user);

            var student = new Student
            {
                UserId = user.Id
            };

            foreach (var teacherId in teachersId)
            {
                var studentTeacher = new StudentTeacher
                {
                    TeacherId = teacherId,
                    StudentId = user.Id
                };

                student.StudentTeachers.Add(studentTeacher);
            }

            Database.StudentRepository.Create(student);

            Database.Save();
        }

        public IEnumerable<StudentBLL> GetStudents()
        {
            return Database.StudentRepository.GetAll().Select(user => new StudentBLL
            {
                Id = user.StudentId,
                UserId = user.UserId
            });
        }

        public void UpdateStudent(UserBLL userBll, IEnumerable<int> teachersId)
        {
            var user = Database.UserRepository.Get(userBll.Id);

            if (user == null)
                throw new ValidationException("Такого пользователя больше нету", "");

            user.RoleId = userBll.RoleId;
            user.FirstName = userBll.FirstName;
            user.LastName = userBll.LastName;
            user.Login = userBll.Login;
            user.Password = userBll.Password;

            Database.UserRepository.Update(user);

            var student = Database.StudentRepository.GetAll().First(s => s.UserId == userBll.Id);

            foreach (var teacherId in teachersId)
            {
                var studentTeacher = new StudentTeacher
                {
                    TeacherId = teacherId,
                    StudentId = user.Id
                };

                if (student.StudentTeachers.Contains(studentTeacher))
                    continue;

                student.StudentTeachers.Add(studentTeacher);
            }

            Database.StudentRepository.Update(student);

            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
