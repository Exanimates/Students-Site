using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Students_Site.BLL.Services;
using Students_Site.Models.Home;

namespace Students_Site.ViewComponents
{
    public class Footer : ViewComponent
    {
        private readonly ITeacherService _teacherService;
        private readonly IStudentService _studentService;

        public Footer(IStudentService studentService, ITeacherService teacherService)
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

            return View("Footer", indexModel);
        }
    }
}