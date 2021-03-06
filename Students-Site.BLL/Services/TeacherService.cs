﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Students_Site.DAL.Entities;
using Common.Encryption;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.DAL.Enums;
using Students_Site.DAL.Infrastructure;

namespace Students_Site.BLL.Services
{
    public interface ITeacherService : IDisposable
    {
        void Create(TeacherBLL userBll);
        void Update(TeacherBLL userBll);
        TeacherBLL Get(int id);
        IEnumerable<TeacherBLL> GetAll();
    }

    public class TeacherService: ITeacherService
    {
        private IUnitOfWork _unitOfWork { get; }

        public TeacherService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Create(TeacherBLL teacherBll)
        {
            if (_unitOfWork.UserRepository.Find(u => u.Login == teacherBll.User.Login).Any())
                throw new ValidationException("Пользователь с таким логином уже существует","");

            foreach (var student in teacherBll.Students)
            {
                if (student.Teachers.GroupBy(st => st.SubjectName).Any(st => st.Key == teacherBll.SubjectName))
                    throw new ValidationException($"Нельзя добавить преподавателя для {student.User.FirstName}. У него уже ведут предмет {teacherBll.SubjectName}", "");
            }

            var salt = Salt.Create();
            var user = new User
            {
                FirstName = teacherBll.User.FirstName,
                LastName = teacherBll.User.LastName,
                Login = teacherBll.User.Login,
                Role = Roles.Teacher,

                Salt = salt,
                Password = Hash.Create(teacherBll.User.Password, salt),
            };

            _unitOfWork.UserRepository.Create(user);

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

            _unitOfWork.TeacherRepository.Create(teacher);

            _unitOfWork.Save();
        }

        public IEnumerable<TeacherBLL> GetAll()
        {
            var users = _unitOfWork.UserRepository.GetAll().Select(user => new UserBLL
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Login = user.Login,
                Role = Roles.Teacher
            });

            return _unitOfWork.TeacherRepository.GetAll().Select(teacher => new TeacherBLL
            {
                Id = teacher.Id,

                SubjectId = teacher.SubjectId,
                SubjectName = teacher.Subject.Name,

                UserId = teacher.UserId,
                User = users.FirstOrDefault(u => u.Id == teacher.UserId),

                Students = teacher.StudentTeachers.Select(studentTeachers => new StudentBLL
                {
                    Id = studentTeachers.StudentId,

                    UserId = studentTeachers.Student.UserId,
                    User = users.FirstOrDefault(u => u.Id == studentTeachers.Student.UserId)
                }).ToArray()
            }).ToArray();
        }

        public void Update(TeacherBLL teacherBll)
        {
            if (_unitOfWork.UserRepository.Find(u => u.Login == teacherBll.User.Login && u.Id != teacherBll.User.Id).Any())
                throw new ValidationException("Пользователь с таким логином уже существует", "");

            foreach (var student in teacherBll.Students)
            {
                if (student.Teachers.GroupBy(t => t.SubjectName).Any(st => st.Key == teacherBll.SubjectName && st.Any(t => t.Id != teacherBll.Id ) ))
                    throw new ValidationException($"Нельзя добавить преподавателя для {student.User.FirstName}. У него уже ведут предмет {teacherBll.SubjectName}", "");
            }

            var user = _unitOfWork.UserRepository.Get(teacherBll.User.Id);

            if (user == null)
                throw new ValidationException("Такого пользователя больше нету", "");

            user.Role = Roles.Teacher;
            user.Id = teacherBll.User.Id;
            user.FirstName = teacherBll.User.FirstName;
            user.LastName = teacherBll.User.LastName;
            user.Login = teacherBll.User.Login;

            if (teacherBll.User.Password != null)
            {
                user.Salt = Salt.Create();
                user.Password = Hash.Create(teacherBll.User.Password, user.Salt);
            }

            _unitOfWork.UserRepository.Update(user);

            var currentTeacher = Get(teacherBll.Id);

            foreach (var newStudent in teacherBll.Students)
            {
                if (currentTeacher.Students.All(t => t.Id != newStudent.Id))
                {
                    _unitOfWork.StudentTeacherRepository.Create(new StudentTeacher
                    {
                        TeacherId = teacherBll.Id,
                        StudentId = newStudent.Id
                    });
                }
            }

            foreach (var oldStudent in currentTeacher.Students)
            {
                if (teacherBll.Students.All(t => t.Id != oldStudent.Id))
                {
                    _unitOfWork.StudentTeacherRepository.Delete(oldStudent.Id, teacherBll.Id);
                }
            }

            var teacherDal = _unitOfWork.TeacherRepository.Get(teacherBll.Id);

            teacherDal.SubjectId = teacherBll.SubjectId;

            _unitOfWork.TeacherRepository.Update(teacherDal);

            _unitOfWork.Save();
        }

        public TeacherBLL Get(int id)
        {
            var users = _unitOfWork.UserRepository.GetAll().Select(user => new UserBLL
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Login = user.Login,
                Role = Roles.Teacher,
            });

            var teacher = _unitOfWork.TeacherRepository.Get(id);

            if (teacher == null)
                throw new ValidationException("Преподаватель не найден", "");

            var studentsTeacher = _unitOfWork.StudentTeacherRepository.Find(st => st.TeacherId == id);

            var teacherBll = new TeacherBLL
            {
                Id = teacher.Id,

                SubjectId = teacher.SubjectId,

                UserId = teacher.UserId,
                User = users.FirstOrDefault(u => u.Id == teacher.UserId),

                Students = studentsTeacher.Select(ts => new StudentBLL
                {
                    Id = ts.StudentId,

                    UserId = ts.Student.UserId,
                    User = users.FirstOrDefault(u => u.Id == ts.Student.UserId)
                }).ToArray()
            };

            return teacherBll;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}