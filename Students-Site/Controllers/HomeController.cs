using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.Services;
using Students_Site.Models;

namespace Students_Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IStudentService _studentService;

        public HomeController(ITeacherService teacherService, IStudentService studentService)
        {
            _teacherService = teacherService;
            _studentService = studentService;
        }

        public IActionResult Index()
        {
            return View();
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

        protected override void Dispose(bool disposing)
        {
            _teacherService.Dispose();
            _studentService.Dispose();
            base.Dispose(disposing);
        }
    }
}