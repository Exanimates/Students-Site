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
            if (_database.UserRepository.Find(u => u.Login == studentBll.User.Login).Any())
                throw new ValidationException("Пользователь с таким логином уже существует", "");

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

            var students = _database.StudentRepository.GetAll().Select(student => new StudentBLL
            {
                Id = student.Id,
                UserId = student.UserId,

                User = users.FirstOrDefault(u => u.Id == student.UserId),
                Teachers = student.StudentTeachers.Select(studentTeachers => new TeacherBLL
                {
                    Id = studentTeachers.TeacherId,
                    UserId = studentTeachers.Teacher.UserId,
                    SubjectId = studentTeachers.Teacher.SubjectId,
                    Grade = studentTeachers.Grade,
                    User = users.FirstOrDefault(u => u.Id == studentTeachers.Teacher.UserId)
                }),
            }).ToArray();

            foreach(var student in students)
            {
                if (!student.Teachers.Any())
                {
                    student.AverageScore = 0;
                    continue;
                }
                student.AverageScore = student.Teachers.Sum(t => t.Grade) / student.Teachers.Count();
            }

            return students;
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
                    User = users.FirstOrDefault(u => u.Id == ts.Teacher.UserId),
                    Grade = ts.Grade
                }),
            };

            studentBll.AverageScore = studentBll.Teachers.Sum(t => t.Grade) / studentBll.Teachers.Count();

            return studentBll;
        }

        public void UpdateStudent(StudentBLL studentBll)
        {
            if (_database.UserRepository.Find(u => u.Login == studentBll.User.Login && u.Id != studentBll.User.Id).Any())
                throw new ValidationException("Пользователь с таким логином уже существует", "");
    
            var user = _database.UserRepository.Get(studentBll.User.Id);

            if (user == null)
                throw new ValidationException("Такого пользователя больше нету", "");

            user.RoleId = 2;
            user.Id = studentBll.User.Id;
            user.FirstName = studentBll.User.FirstName;
            user.LastName = studentBll.User.LastName;
            user.Login = studentBll.User.Login;
            user.Password = studentBll.User.Password;

            _database.UserRepository.Update(user);

            var student = _database.StudentRepository.Get(studentBll.Id);

            student.Id = studentBll.Id;

            _database.StudentRepository.Update(student);

            var allStudentTeachers = _database.StudentTeacherRepository.GetAll().ToArray();

            foreach (var currentTeacher in studentBll.Teachers)
            {
                if (!allStudentTeachers.Any(st => st.StudentId == studentBll.Id && st.TeacherId == currentTeacher.Id) &&
                    currentTeacher.IsSelected)
                {
                    _database.StudentTeacherRepository.Create(new StudentTeacher
                    {
                        TeacherId = currentTeacher.Id,
                        StudentId = studentBll.Id,
                        Grade = currentTeacher.Grade
                    });
                }

                if (allStudentTeachers.Any(st => st.StudentId == studentBll.Id && st.TeacherId == currentTeacher.Id) && !currentTeacher.IsSelected)
                    _database.StudentTeacherRepository.Delete(studentBll.Id, currentTeacher.Id);

                if (allStudentTeachers.Any(st => st.StudentId == studentBll.Id && st.TeacherId == currentTeacher.Id) &&
                    currentTeacher.IsSelected)
                {
                    var studentTeacher = _database.StudentTeacherRepository.Get(studentBll.Id, currentTeacher.Id);
                    studentTeacher.Grade = currentTeacher.Grade;

                    _database.StudentTeacherRepository.Update(studentTeacher);
                }
            }

            _database.Save();
        }

        public void Dispose()
        {
            _database.Dispose();
        }
    }
}