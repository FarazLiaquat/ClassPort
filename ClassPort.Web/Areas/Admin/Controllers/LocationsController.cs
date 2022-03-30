using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RoverCore.BreadCrumbs.Services;
using RoverCore.Datatables.DTOs;
using RoverCore.Datatables.Extensions;
using ClassPort.Web.Controllers;
using ClassPort.Infrastructure.Common.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using ClassPort.Domain.Entities;
using ClassPort.Infrastructure.Persistence.DbContexts;

namespace ClassPort.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class LocationsController : BaseController<LocationsController>
{
	public class LocationIndexViewModel 
	{
		[Key]            
	    public int Id { get; set; }
	    public string Place { get; set; }
	    public string RoomNum { get; set; }
	}

	private const string createBindingFields = "Id,Place,RoomNum";
    private const string editBindingFields = "Id,Place,RoomNum";
    private const string areaTitle = "Admin";

    private readonly ApplicationDbContext _context;

    public LocationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Admin/Locations
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage Location");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<LocationIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Admin/Locations/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Location", "Index", "Locations", new { Area = "Admin" })
            .Then("Location Details");            

        if (id == null)
        {
            return NotFound();
        }

        var location = await _context.Location
            .FirstOrDefaultAsync(m => m.Id == id);
        if (location == null)
        {
            return NotFound();
        }

        return View(location);
    }

    // GET: Admin/Locations/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Location", "Index", "Locations", new { Area = "Admin" })
            .Then("Create Location");     

       return View();
	}

    // POST: Admin/Locations/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] Location location)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Location", "Index", "LocationsController", new { Area = "Admin" })
        .Then("Create Location");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            _context.Add(location);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
            }
        return View(location);
    }

    // GET: Admin/Locations/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Location", "Index", "Locations", new { Area = "Admin" })
        .Then("Edit Location");     

        if (id == null)
        {
            return NotFound();
        }

        var location = await _context.Location.FindAsync(id);
        if (location == null)
        {
            return NotFound();
        }
        

        return View(location);
    }

    // POST: Admin/Locations/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(editBindingFields)] Location location)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Location", "Index", "Locations", new { Area = "Admin" })
        .Then("Edit Location");  
    
        if (id != location.Id)
        {
            return NotFound();
        }
        
        Location model = await _context.Location.FindAsync(id);

        model.Place = location.Place;
        model.RoomNum = location.RoomNum;
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(editBindingFields);           

        if (ModelState.IsValid)
        {
            try
            {
                await _context.SaveChangesAsync();
                _toast.Success("Updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(location.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(location);
    }

    // GET: Admin/Locations/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Location", "Index", "Locations", new { Area = "Admin" })
        .Then("Delete Location");  

        if (id == null)
        {
            return NotFound();
        }

        var location = await _context.Location
            .FirstOrDefaultAsync(m => m.Id == id);
        if (location == null)
        {
            return NotFound();
        }

        return View(location);
    }

    // POST: Admin/Locations/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var location = await _context.Location.FindAsync(id);
        _context.Location.Remove(location);
        await _context.SaveChangesAsync();
        
        _toast.Success("Location deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool LocationExists(int id)
    {
        return _context.Location.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetLocation(DtRequest request)
    {
        try
		{
			var query = _context.Location;
			var jsonData = await query.GetDatatableResponseAsync<Location, LocationIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Location index json");
        }
        
        return StatusCode(500);
    }

}

