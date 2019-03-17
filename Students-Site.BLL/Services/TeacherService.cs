using Students_Site.BLL.Business_Logic_Models;
using Students_Site.BLL.Helpers;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Students_Site.BLL.Interfaces;

namespace Students_Site.BLL.Services
{
    public class TeacherService: ITeacherService
    {
        IUnitOfWork Database { get; set; }

        public TeacherService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public void MakeTeacher(UserBLL userBll, IEnumerable<int> studentsId)
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

            var teacher = new Teacher
            {
                UserId = user.Id
            };

            foreach (var teacherId in studentsId)
            {
                var studentTeacher = new StudentTeacher
                {
                    TeacherId = teacherId,
                    StudentId = user.Id
                };

                teacher.StudentTeachers.Add(studentTeacher);
            }

            Database.TeacherRepository.Create(teacher);

            Database.Save();
        }

        public IEnumerable<TeacherBLL> GetTeachers()
        {
            return Database.StudentRepository.GetAll().Select(user => new TeacherBLL
            {
                Id = user.Id,
                UserId = user.Id
            });
        }

        public void UpdateTeacher(UserBLL userBll, IEnumerable<int> teachersId)
        {
            var user = Database.UserRepository.Get(userBll.Id);

            if (user == null) {
                throw new ValidationException("Такого пользователя больше нету", "");
            }

            user.RoleId = userBll.RoleId;
            user.FirstName = userBll.FirstName;
            user.LastName = userBll.LastName;
            user.Login = userBll.Login;
            user.Password = userBll.Password;

            Database.UserRepository.Update(user);

            var teacher = Database.TeacherRepository.GetAll().First(s => s.UserId == userBll.Id);

            foreach (var teacherId in teachersId)
            {
                var studentTeacher = new StudentTeacher
                {
                    TeacherId = teacherId,
                    StudentId = user.Id
                };

                if (teacher.StudentTeachers.Contains(studentTeacher)) {
                    continue;
                }

                teacher.StudentTeachers.Add(studentTeacher);
            }

            Database.TeacherRepository.Update(teacher);

            Database.Save();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
