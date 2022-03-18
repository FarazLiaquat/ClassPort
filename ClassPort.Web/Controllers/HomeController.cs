using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClassPort.Web.Controllers;

[AllowAnonymous]
public class HomeController : BaseController<HomeController>
{
    public IActionResult Index()
    {
        return View();
    }

    // Template actions
    public IActionResult About() => View();
    public IActionResult TOS() => View();
    public IActionResult Privacy() => View();
}