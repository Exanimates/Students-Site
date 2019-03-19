using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.Services;
using Students_Site.Models;

namespace Students_Site.Controllers
{
    public class TeachersController : Controller
    {
        ITeacherService _teacherService;
        IUserService _userService;
        ISubjectService _subjectService;

        public TeachersController(ITeacherService teacherService, IUserService userService, ISubjectService subjectService)
        {
            _teacherService = teacherService;
            _userService = userService;
            _subjectService = subjectService;
        }

        public IActionResult Index()
        {
            var teacherBll = _teacherService.GetTeachers();

            var users = _userService.GetUsers();

            var teachers = teacherBll
                .Join(
                    users,
                    t => t.UserId,
                    u => u.Id,
                    (t, u) => new TeacherModel
                    {
                        Id = t.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Login = u.Login,
                        Password = u.Password,
                        RoleId = u.RoleId,
                        SubjectId = t.SubjectId,
                        SubjectName = _subjectService.GetSubject(t.SubjectId).Name
                    });

            return View(teachers);
        }

        protected override void Dispose(bool disposing)
        {
            _teacherService.Dispose();
            _userService.Dispose();
            base.Dispose(disposing);
        }
    }
}