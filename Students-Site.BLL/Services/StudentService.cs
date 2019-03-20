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
        void MakeStudent(UserBLL userBll, IEnumerable<int> teachersId, int roleId);
        void UpdateStudent(UserBLL userBll, IEnumerable<int> teachersId);
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

        public void MakeStudent(UserBLL userBll, IEnumerable<int> teachersId, int roleId)
        {
            var user = new User
            {
                FirstName = userBll.FirstName,
                LastName = userBll.LastName,
                Login = userBll.Login,
                Password = userBll.Password,
                RoleId = roleId
            };

            _database.UserRepository.Create(user);

            var student = new Student
            {
                UserId = user.Id
            };

            if (teachersId != null)
            {
                foreach (var teacherId in teachersId)
                {
                    var studentTeacher = new StudentTeacher
                    {
                        TeacherId = teacherId,
                        StudentId = user.Id
                    };

                    student.StudentTeachers.Add(studentTeacher);
                }
            }

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
                throw new ValidationException("Id студента не установлено", "");

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