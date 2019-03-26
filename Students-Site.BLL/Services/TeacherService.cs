using Students_Site.DAL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.BLL.Services
{
    public interface ITeacherService : IService
    {
        void MakeTeacher(TeacherBLL userBll);
        void UpdateTeacher(TeacherBLL userBll);
        TeacherBLL GetTeacher(int id);
        IEnumerable<TeacherBLL> GetTeachers();
    }

    public class TeacherService: ITeacherService
    {
        IUnitOfWork _database { get; set; }

        public TeacherService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
        }

        public void MakeTeacher(TeacherBLL teacherBll)
        {
            if (_database.UserRepository.Find(u => u.Login == teacherBll.User.Login).Any())
                throw new ValidationException("Пользователь с таким логином уже существует","");

            foreach (var student in teacherBll.Students)
            {
                if (student.Teachers.GroupBy(st => st.SubjectName == teacherBll.SubjectName).Any())
                {
                    throw new ValidationException($"Нельзя добавить преподавателя для {student.User.FirstName}. У него уже ведут предмет {teacherBll.SubjectName}", "");
                }
            }

            var user = new User
            {
                FirstName = teacherBll.User.FirstName,
                LastName = teacherBll.User.LastName,
                Login = teacherBll.User.Login,
                Password = teacherBll.User.Password,
                RoleId = 3
            };

            _database.UserRepository.Create(user);

            var teacher = new Teacher
            {
                UserId = user.Id,
                SubjectId = teacherBll.SubjectId,

                StudentTeachers = teacherBll.Students.Select(s => new StudentTeacher
                {
                    TeacherId = teacherBll.Id,
                    StudentId = s.Id
                }).ToArray()
            };

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
                    User = users.FirstOrDefault(u => u.Id == studentTeachers.Student.UserId),
                })
            }).ToArray();
        }

        public void UpdateTeacher(TeacherBLL teacherBll)
        {
            if (_database.UserRepository.Find(u => u.Login == teacherBll.User.Login && u.Id != teacherBll.User.Id).Any())
                throw new ValidationException("Пользователь с таким логином уже существует", "");

            var user = _database.UserRepository.Get(teacherBll.User.Id);

            if (user == null)
                throw new ValidationException("Такого пользователя больше нету", "");

            user.RoleId = 3;
            user.Id = teacherBll.User.Id;
            user.FirstName = teacherBll.User.FirstName;
            user.LastName = teacherBll.User.LastName;
            user.Login = teacherBll.User.Login;
            user.Password = teacherBll.User.Password;

            _database.UserRepository.Update(user);

            var teacher = _database.TeacherRepository.Get(teacherBll.Id);

            teacher.SubjectId = teacherBll.SubjectId;

            _database.TeacherRepository.Update(teacher);

            var allStudentTeachers = _database.StudentTeacherRepository.GetAll().ToArray();

            foreach (var currentStudent in teacherBll.Students)
            {
                if (!allStudentTeachers.Any(st => st.StudentId == currentStudent.Id && st.TeacherId == teacherBll.Id) &&
                    currentStudent.IsSelected)
                {
                    _database.StudentTeacherRepository.Create(new StudentTeacher
                    {
                        TeacherId = teacherBll.Id,
                        StudentId = currentStudent.Id
                    });
                }
                if (allStudentTeachers.Any(st => st.StudentId == currentStudent.Id && st.TeacherId == teacherBll.Id) && !currentStudent.IsSelected)
                    _database.StudentTeacherRepository.Delete(currentStudent.Id, teacherBll.Id);
            }

            _database.Save();
        }

        public TeacherBLL GetTeacher(int id)
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

            var teacher = _database.TeacherRepository.Get(id);

            if (teacher == null)
                throw new ValidationException("Преподаватель не найден", "");

            var studentsTeacher = _database.StudentTeacherRepository.Find(st => st.TeacherId == id);

            var teacherBll = new TeacherBLL
            {
                Id = teacher.Id,
                UserId = teacher.UserId,
                SubjectId = teacher.SubjectId,

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