using System;
using System.Collections.Generic;
using System.Linq;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Enums;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Entities;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.BLL.Services
{
    public interface IStudentService : IDisposable
    {
        void Create(StudentBLL studentBll);
        void Update(StudentBLL studentBll);
        StudentBLL Get(int id);
        IEnumerable<StudentBLL> GetAll();
    }

    public class StudentService : IStudentService
    {
        private IUnitOfWork _unitOfWork { get; }

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(StudentBLL studentBll)
        {
            if (_unitOfWork.UserRepository.Find(u => u.Login == studentBll.User.Login).Any())
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
                RoleId = (int) Roles.Student
            };

            _unitOfWork.UserRepository.Create(user);

            var student = new Student
            {
                UserId = user.Id,

                StudentTeachers = studentBll.Teachers.Select(t => new StudentTeacher
                {
                    TeacherId = t.Id,
                    StudentId = studentBll.Id,
                    Grade = t.Grade
                }).ToList()
            };

            _unitOfWork.StudentRepository.Create(student);

            _unitOfWork.Save();
        }

        public IEnumerable<StudentBLL> GetAll()
        {
            var users = _unitOfWork.UserRepository.GetAll().Select(user => new UserBLL
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Login = user.Login,
                RoleId = user.RoleId
            });

            var students = _unitOfWork.StudentRepository.GetAll().Select(student => new StudentBLL
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
                student.AverageScore = student.Teachers.Sum(t => t.Grade) / student.Teachers.Count();
            }

            return students.ToArray();
        }

        public StudentBLL Get(int id)
        {
            var users = _unitOfWork.UserRepository.GetAll().Select(user => new UserBLL
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Login = user.Login,
                RoleId = user.RoleId
            });

            var student = _unitOfWork.StudentRepository.Get(id);

            if (student == null)
                throw new ValidationException("Студент не найден", "");

            var teachersStudent = _unitOfWork.StudentTeacherRepository.Find(st => st.StudentId == id);

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

        public void Update(StudentBLL studentBll)
        {
            if (_unitOfWork.UserRepository.Find(u => u.Login == studentBll.User.Login && u.Id != studentBll.User.Id).Any())
                throw new ValidationException("Пользователь с таким логином уже существует", "");
    
            var user = _unitOfWork.UserRepository.Get(studentBll.User.Id);

            if (user == null)
                throw new ValidationException("Такого пользователя больше нету", "");

            user.RoleId = 2;
            user.Id = studentBll.User.Id;
            user.FirstName = studentBll.User.FirstName;
            user.LastName = studentBll.User.LastName;
            user.Login = studentBll.User.Login;
            user.Password = studentBll.User.Password;

            _unitOfWork.UserRepository.Update(user);

            var studentDal = Get(studentBll.Id);

            foreach (var newTeacher in studentBll.Teachers)
            {
                if (studentDal.Teachers.Any(t => t.Id == newTeacher.Id))
                {
                    var studentTeacher = _unitOfWork.StudentTeacherRepository.Get(studentBll.Id, newTeacher.Id);
                    studentTeacher.Grade = newTeacher.Grade;

                    _unitOfWork.StudentTeacherRepository.Update(studentTeacher);
                }
                else
                {
                    _unitOfWork.StudentTeacherRepository.Create(new StudentTeacher
                    {
                        TeacherId = newTeacher.Id,
                        StudentId = studentBll.Id,
                        Grade = newTeacher.Grade
                    });
                }
            }

            foreach (var oldTeacher in studentDal.Teachers)
            {
                if (studentBll.Teachers.All(t => t.Id != oldTeacher.Id))
                {
                    _unitOfWork.StudentTeacherRepository.Delete(studentBll.Id, oldTeacher.Id);
                }
            }

            _unitOfWork.Save();
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}