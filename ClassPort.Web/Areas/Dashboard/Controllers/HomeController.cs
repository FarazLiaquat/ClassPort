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

        return View(viewModel);
    }
}