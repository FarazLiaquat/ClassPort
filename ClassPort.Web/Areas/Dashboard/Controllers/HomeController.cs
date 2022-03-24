using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ClassPort.Domain.Entities.Identity;
using ClassPort.Web.Areas.Dashboard.Models.HomeViewModels;
using ClassPort.Web.Controllers;
using RoverCore.BreadCrumbs.Services;
using System.Threading.Tasks;

namespace ClassPort.Web.Areas.Dashboard.Controllers;

[Area("Dashboard")]
[Authorize]
[ApiExplorerSettings(IgnoreApi = true)]
public class HomeController : BaseController<HomeController>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    public HomeController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .Then("Home");

        var viewModel = new HomeViewModel
        {
            User = await _userManager.GetUserAsync(User)
        };
        _toast.Success($"Welcome back {viewModel.User.FirstName}!");
        if (User.IsInRole("SchoolAdmin"))
        {
            return RedirectToAction("SchoolAdmin");
        }
        if (User.IsInRole("Teacher"))
        {
            return RedirectToAction("Teacher");
        }
        if (User.IsInRole("Student"))
        {
            return RedirectToAction("Student");
        }
        return View(viewModel);
    }
    public async Task<IActionResult> Teacher()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .Then("Teacher");
        var viewModel = new HomeViewModel
        {
            User = await _userManager.GetUserAsync(User)
        };
        var roles = _roleManager.GetRoleNameAsync;
        _toast.Success($"Welcome back {viewModel.User.FirstName}!");
        return View(viewModel);
    }
    public async Task<IActionResult> SchoolAdmin()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .Then("SchoolAdmin");
        var viewModel = new HomeViewModel
        {
            User = await _userManager.GetUserAsync(User)
        };
        var roles = _roleManager.GetRoleNameAsync;
        _toast.Success($"Welcome back {viewModel.User.FirstName}!");
        return View(viewModel);
    }
    public async Task<IActionResult> Student()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .Then("Students");
        var viewModel = new HomeViewModel
        {
            User = await _userManager.GetUserAsync(User)
        };
        var roles = _roleManager.GetRoleNameAsync;
        _toast.Success($"Welcome back {viewModel.User.FirstName}!");
        return View(viewModel);
    }

  }