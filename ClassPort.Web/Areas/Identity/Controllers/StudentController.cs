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

namespace ClassPort.Web.Areas.Identity.Controllers;

[Area("Identity")]
[Authorize(Roles = "Admin")]
public class StudentController : BaseController<StudentController>
{
	public class StudentIndexViewModel 
	{
		[Key]            
	    public int Id { get; set; }
	    public DateTime Created { get; set; }
	    public int Time { get; set; }
        public string Name { get; set; }
	}

	private const string createBindingFields = "Id,Created,Time,Name";
    private const string editBindingFields = "Id,Created,Time,Name";
    private const string areaTitle = "Identity";

    private readonly ApplicationDbContext _context;

    public StudentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Identity/Student
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
			.Then("Manage Student");       
		
		// Fetch descriptive data from the index dto to build the datatables index
		var metadata = DatatableExtensions.GetDtMetadata<StudentIndexViewModel>();
		
		return View(metadata);
   }

    // GET: Identity/Student/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Student", "Index", "Student", new { Area = "Identity" })
            .Then("Student Details");            

        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Student
            .FirstOrDefaultAsync(m => m.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // GET: Identity/Student/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage Student", "Index", "Student", new { Area = "Identity" })
            .Then("Create Student");     

       return View();
	}

    // POST: Identity/Student/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] Student student)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Student", "Index", "StudentController", new { Area = "Identity" })
        .Then("Create Student");     
        
        // Remove validation errors from fields that aren't in the binding field list
        ModelState.Scrub(createBindingFields);           

        if (ModelState.IsValid)
        {
            _context.Add(student);
            await _context.SaveChangesAsync();
            
            _toast.Success("Created successfully.");
            
                return RedirectToAction(nameof(Index));
            }
        return View(student);
    }

    // GET: Identity/Student/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Student", "Index", "Student", new { Area = "Identity" })
        .Then("Edit Student");     

        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Student.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }
        

        return View(student);
    }

    // POST: Identity/Student/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(editBindingFields)] Student student)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Student", "Index", "Student", new { Area = "Identity" })
        .Then("Edit Student");  
    
        if (id != student.Id)
        {
            return NotFound();
        }
        
        Student model = await _context.Student.FindAsync(id);

        model.Created = student.Created;
        model.Time = student.Time;
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
                if (!StudentExists(student.Id))
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
        return View(student);
    }

    // GET: Identity/Student/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage Student", "Index", "Student", new { Area = "Identity" })
        .Then("Delete Student");  

        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Student
            .FirstOrDefaultAsync(m => m.Id == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // POST: Identity/Student/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _context.Student.FindAsync(id);
        _context.Student.Remove(student);
        await _context.SaveChangesAsync();
        
        _toast.Success("Student deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool StudentExists(int id)
    {
        return _context.Student.Any(e => e.Id == id);
    }


	[HttpPost]
	[ValidateAntiForgeryToken]
    public async Task<IActionResult> GetStudent(DtRequest request)
    {
        try
		{
			var query = _context.Student;
			var jsonData = await query.GetDatatableResponseAsync<Student, StudentIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating Student index json");
        }
        
        return StatusCode(500);
    }

}

