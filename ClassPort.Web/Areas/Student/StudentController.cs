using Microsoft.AspNetCore.Mvc;

namespace ClassPort.Web.Areas.Student
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
