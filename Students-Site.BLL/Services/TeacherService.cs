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
        TeacherBLL GetTeacher(int? id);
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
            var users = _database.UserRepository.GetAll().Select(user => new UserBLL
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Login = user.Login,
                RoleId = user.RoleId
            });

            return _database.TeacherRepository.GetAll().Select(teacher => new TeacherBLL
            {
                Id = teacher.Id,
                UserId = teacher.UserId,
                SubjectId = teacher.SubjectId,
                SubjectName = teacher.Subject.Name,

                User = users.FirstOrDefault(u => u.Id == teacher.UserId),
                Students = teacher.StudentTeachers.Select(studentTeachers => new StudentBLL
                {
                    Id = studentTeachers.StudentId,
                    UserId = studentTeachers.Student.UserId,
                    User = users.FirstOrDefault(u => u.Id == studentTeachers.Student.UserId)
                })
            }).ToArray();
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

        public TeacherBLL GetTeacher(int? id)
        {
            var users = _database.UserRepository.GetAll().Select(user => new UserBLL
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Login = user.Login,
                RoleId = user.RoleId
            });

            if (id == null)
                throw new ValidationException("Id преподавателя не установлено", "");

            var teacher = _database.TeacherRepository.Get(id);

            if (teacher == null)
                throw new ValidationException("Преподаватель не найден", "");

            var studentsTeacher = _database.StudentTeacherRepository.Find(st => st.TeacherId == id);

            var teacherBll = new TeacherBLL
            {
                Id = teacher.Id,
                UserId = teacher.UserId,

                User = users.FirstOrDefault(u => u.Id == teacher.UserId),
                Students = studentsTeacher.Select(ts => new StudentBLL
                {
                    Id = ts.StudentId,
                    UserId = ts.Student.UserId,
                    User = users.FirstOrDefault(u => u.Id == ts.Student.UserId)
                })
            };

            return teacherBll;
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}