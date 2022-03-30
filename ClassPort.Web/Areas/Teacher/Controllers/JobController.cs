using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClassPort.Web.Controllers;
using RoverCore.BreadCrumbs.Services;

namespace ClassPort.Web.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles="Teacher")]
    public class JobController : BaseController<JobController>
    {
        public IActionResult Index()
        {
	        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
		        .Then("Teacher")
		        .ThenAction("Hangfire Dashboard", "Index", "Job", new { Area = "Admin" });

            return View();
        }
    }
}
