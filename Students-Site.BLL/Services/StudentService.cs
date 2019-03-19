using System.Collections.Generic;
using System.Linq;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.BLL.Services
{
    public interface IStudentService : IService
    {
        void MakeStudent(UserBLL userBll, IEnumerable<int> teachersId);
        void UpdateStudent(UserBLL userBll, IEnumerable<int> teachersId);
        IEnumerable<StudentBLL> GetStudents();
    }

    public class StudentService : IStudentService
    {
        IUnitOfWork _database { get; set; }

        public StudentService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
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

            _database.UserRepository.Create(user);

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

            _database.StudentRepository.Create(student);

            _database.Save();
        }

        public IEnumerable<StudentBLL> GetStudents()
        {
            return _database.StudentRepository.GetAll().Select(user => new StudentBLL
            {
                Id = user.Id,
                UserId = user.UserId
            }).ToArray();
        }

        public void UpdateStudent(UserBLL userBll, IEnumerable<int> teachersId)
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

            var student = _database.StudentRepository.GetAll().First(s => s.UserId == userBll.Id);

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

            _database.StudentRepository.Update(student);

            _database.Save();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}
