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
        void MakeStudent(StudentBLL studentBll);
        void UpdateStudent(StudentBLL studentBll);
        StudentBLL GetStudent(int? id);
        IEnumerable<StudentBLL> GetStudents();
    }

    public class StudentService : IStudentService
    {
        IUnitOfWork _database { get; set; }

        public StudentService(IUnitOfWork unitOfWork)
        {
            _database = unitOfWork;
        }

        public void MakeStudent(StudentBLL studentBll)
        {
            var teachersGroupBySubject = studentBll.Teachers.GroupBy(t => t.SubjectName);

            if (teachersGroupBySubject.Any(subject => subject.Count() > 1))
                throw new ValidationException("Нельзя добавить несколько преподавателей одного предмета", "");

            var user = new User
            {
                FirstName = studentBll.User.FirstName,
                LastName = studentBll.User.LastName,
                Login = studentBll.User.Login,
                Password = studentBll.User.Password,
                RoleId = 2
            };

            _database.UserRepository.Create(user);

            var student = new Student
            {
                UserId = user.Id,

                StudentTeachers = studentBll.Teachers.Select(t => new StudentTeacher
                {
                    TeacherId = t.Id,
                    StudentId = studentBll.Id
                }).ToList()
            };

            _database.StudentRepository.Create(student);

            _database.Save();
        }

        public IEnumerable<StudentBLL> GetStudents()
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

            return _database.StudentRepository.GetAll().Select(student => new StudentBLL
            {
                Id = student.Id,
                UserId = student.UserId,

                User = users.FirstOrDefault(u => u.Id == student.UserId),
                Teachers = student.StudentTeachers.Select(studentTeachers => new TeacherBLL
                {
                    Id = studentTeachers.TeacherId,
                    UserId = studentTeachers.Teacher.UserId,
                    SubjectId = studentTeachers.Teacher.SubjectId,
                    User = users.FirstOrDefault(u => u.Id == studentTeachers.Teacher.UserId)
                })
            }).ToArray();
        }

        public StudentBLL GetStudent(int? id)
        {
            if (id == null)
                throw new ValidationException("Id студента не установлено", "");

            var users = _database.UserRepository.GetAll().Select(user => new UserBLL
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Login = user.Login,
                RoleId = user.RoleId
            });

            var student = _database.StudentRepository.Get(id);

            if (student == null)
                throw new ValidationException("Студент не найден", "");

            var teachersStudent = _database.StudentTeacherRepository.Find(st => st.StudentId == id);

            var studentBll = new StudentBLL
            {
                Id = student.Id,
                UserId = student.UserId,

                User = users.FirstOrDefault(u => u.Id == student.UserId),
                Teachers = teachersStudent.Select(ts => new TeacherBLL
                {
                    Id = ts.TeacherId,
                    UserId = ts.Teacher.UserId,
                    SubjectId = ts.Teacher.SubjectId,
                    SubjectName = ts.Teacher.Subject.Name,
                    User = users.FirstOrDefault(u => u.Id == ts.Teacher.UserId)
                })
            };

            return studentBll;
        }

        public void UpdateStudent(StudentBLL studentBll)
        {
            var user = _database.UserRepository.Get(studentBll.Id);

            if (user == null)
                throw new ValidationException("Такого пользователя больше нету", "");

            user.FirstName = studentBll.User.FirstName;
            user.LastName = studentBll.User.LastName;
            user.Login = studentBll.User.Login;
            user.Password = studentBll.User.Password;

            _database.UserRepository.Update(user);

            var student = _database.StudentRepository.GetAll().First(s => s.UserId == studentBll.User.Id);

            _database.StudentRepository.Update(student);

            _database.Save();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}