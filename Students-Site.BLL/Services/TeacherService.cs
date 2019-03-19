using Students_Site.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.BLL.Services
{
    public interface ITeacherService : IService
    {
        void MakeTeacher(UserBLL userBll, IEnumerable<int> studentsId);
        void UpdateTeacher(UserBLL userBll, IEnumerable<int> studentsId);
        IEnumerable<TeacherBLL> GetTeachers();
    }

    public class TeacherService: ITeacherService
    {
        IUnitOfWork _database { get; set; }

        public TeacherService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
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

            _database.UserRepository.Create(user);

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

            _database.TeacherRepository.Create(teacher);

            _database.Save();
        }

        public IEnumerable<TeacherBLL> GetTeachers()
        {
            return _database.TeacherRepository.GetAll().Select(teacher => new TeacherBLL
            {
                Id = teacher.Id,
                UserId = teacher.UserId,
                SubjectId = teacher.SubjectId
            });
        }

        public void UpdateTeacher(UserBLL userBll, IEnumerable<int> teachersId)
        {
            var user = _database.UserRepository.Get(userBll.Id);

            if (user == null)
                throw new ValidationException("Такого пользователя больше нету", "");

            user.RoleId = userBll.RoleId;
            user.FirstName = userBll.FirstName;
            user.LastName = userBll.LastName;
            user.Login = userBll.Login;
            user.Password = userBll.Password;

            _database.UserRepository.Update(user);

            var teacher = _database.TeacherRepository.GetAll().First(s => s.UserId == userBll.Id);

            foreach (var teacherId in teachersId)
            {
                var studentTeacher = new StudentTeacher
                {
                    TeacherId = teacherId,
                    StudentId = user.Id
                };

                if (teacher.StudentTeachers.Contains(studentTeacher))
                    continue;

                teacher.StudentTeachers.Add(studentTeacher);
            }

            _database.TeacherRepository.Update(teacher);

            _database.Save();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
