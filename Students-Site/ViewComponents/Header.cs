using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.Services;
using Students_Site.Models.Home;

namespace Students_Site.ViewComponents
{
    public class Header : ViewComponent
    {
        private readonly ITeacherService _teacherService;
        private readonly IStudentService _studentService;

        public Header(IStudentService studentService, ITeacherService teacherService)
        {
            _teacherService = teacherService;
            _studentService = studentService;
        }

        public IViewComponentResult Invoke()
        {
            var indexModel = new IndexModel
            {
                StudentCount = _studentService.GetAll().Count(),
                TeacherCount = _teacherService.GetAll().Count()
            };

            return View("Header", indexModel);
        }
    }
}
