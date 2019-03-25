using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.Services;
using Students_Site.DAL.Infrastructure;
using Students_Site.Models;
using Students_Site.Models.Home;
using Students_Site.Models.Student;

namespace Students_Site.Controllers
{
    public class HomeController : Controller
    {
        readonly ITeacherService _teacherService;
        readonly IStudentService _studentService;

        public HomeController(ITeacherService teacherService, IStudentService studentService)
        {
            _teacherService = teacherService;
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            var studentsModels = _studentService.GetStudents().Count();

            var homeIndex = new IndexModel
            {
                StudentCount = _studentService.GetStudents().Count(),
                TeacherCount = _teacherService.GetTeachers().Count(),
            };

            return View(homeIndex);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}