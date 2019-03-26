﻿using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.BusinessLogicModels;
using Students_Site.BLL.Exceptions;
using Students_Site.BLL.Services;
using Students_Site.Models.Student;
using Students_Site.Models.Subject;
using Students_Site.Models.Teacher;

namespace Students_Site.Controllers
{
    public class TeacherController : Controller
    {
        readonly ITeacherService _teacherService;
        readonly IUserService _userService;
        readonly IStudentService _studentService;
        readonly ISubjectService _subjectService;

        public TeacherController(ITeacherService teacherService, IUserService userService, ISubjectService subjectService, IStudentService studentService)
        {
            _teacherService = teacherService;
            _userService = userService;
            _subjectService = subjectService;
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            var teachersModels = _teacherService.GetAll().Select(s => new TeacherModel
            {
                Id = s.Id,
                FirstName = s.User.FirstName,
                LastName = s.User.LastName,
                Login = s.User.Login,
                Password = s.User.Password,
                RoleId = s.User.RoleId,
                SubjectName = s.SubjectName,

                Students = s.Students.Select(t => new StudentModel
                {
                    Id = t.User.Id,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName
                })
            });

            var teachers = new TeacherIndexModel
            {
                TeacherModels = teachersModels
            };

            return View(teachers);
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult Create()
        {
            var teacher = new TeacherMakeModel
            {
                Students = _studentService.GetAll().Select(s => new StudentModel
                {
                    Id = s.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    IsSelected = false
                }).ToList(),
                Subjects = _subjectService.GetAll().Select(sub => new SubjectModel
                {
                    Id = sub.Id,
                    SubjectName = sub.Name
                }).ToList(),

                SubjectId = 1
            };

            return View(teacher);
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public ActionResult Create(TeacherMakeModel teacher)
        {
            try
            {
                var userBll = new UserBLL
                {
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Login = teacher.Login,
                    Password = teacher.Password
                };

                var teacherBll = new TeacherBLL
                {
                    User = userBll,
                    SubjectId = teacher.SubjectId,
                    SubjectName = teacher.Subjects.First(s => s.Id == teacher.SubjectId).SubjectName,

                    Students = teacher.Students.Where(s => s.IsSelected).Select(s => new StudentBLL
                    {
                        Id = s.Id,
                        UserId = s.UserId,

                        User = new UserBLL
                        {
                            FirstName = s.FirstName,
                        },
                        Teachers = _studentService.Get(s.Id).Teachers
                    }).ToArray()
                };

                _teacherService.Create(teacherBll);

                return Ok("Преподаватель успешно зарегестирован");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "1")]
        public ActionResult Edit(int id)
        {
            var teacherBll = _teacherService.Get(id);

            var teacher = new TeacherEditModel
            {
                Id = teacherBll.Id,
                UserId = teacherBll.UserId,
                FirstName = teacherBll.User.FirstName,
                LastName = teacherBll.User.LastName,
                Login = teacherBll.User.Login,
                Password = teacherBll.User.Password,
                SubjectId = teacherBll.SubjectId,

                Students = _studentService.GetAll().Select(s => new StudentModel
                {
                    Id = s.Id,
                    FirstName = s.User.FirstName,
                    LastName = s.User.LastName,
                    IsSelected = false
                }).ToList(),

                Subjects = _subjectService.GetAll().Select(sub => new SubjectModel
                {
                    Id = sub.Id,
                    SubjectName = sub.Name,
                }).ToList(),

            };

            foreach (var student in teacher.Students)
            {
                if (teacherBll.Students.Any(st => st.Id == student.Id))
                    student.IsSelected = true;
            }            

            return View(teacher);
        }

        [HttpPost]
        [Authorize(Roles = "1")]
        public ActionResult Edit(TeacherEditModel teacher)
        {
            try
            {
                var userBll = new UserBLL
                {
                    Id = teacher.UserId,
                    FirstName = teacher.FirstName,
                    LastName = teacher.LastName,
                    Login = teacher.Login,
                    Password = teacher.Password
                };

                var teacherBll = new TeacherBLL
                {
                    Id = teacher.Id,
                    User = userBll,
                    SubjectId = teacher.SubjectId,

                    Students = teacher.Students.Select(s => new StudentBLL
                    {
                        Id = s.Id,
                        UserId = s.UserId,
                        IsSelected = s.IsSelected
                    })
                };

                _teacherService.Update(teacherBll);

                return Ok("Преподаватель успешно изменен");
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError(ex.Property, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        protected override void Dispose(bool disposing)
        {
            _teacherService.Dispose();
            _userService.Dispose();
            base.Dispose(disposing);
        }

        public IActionResult Show(int id)
        {
            var teacherBll = _teacherService.Get(id);

            var teacher = new TeacherModel
            {
                Id = teacherBll.Id,
                FirstName = teacherBll.User.FirstName,
                LastName = teacherBll.User.LastName,
                Login = teacherBll.User.Login,
                Password = teacherBll.User.Password,
                RoleId = teacherBll.User.RoleId,

                Students = teacherBll.Students.Select(t => new StudentModel
                {
                    Id = t.User.Id,
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName
                })
            };

            return View(teacher);
        }
    }
}