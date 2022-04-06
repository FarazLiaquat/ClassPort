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
public class StudentListController : BaseController<StudentListController>
{
    public class StudentListIndexViewModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public int Time { get; set; }
    }

    private const string createBindingFields = "Id,Created,Time";
    private const string editBindingFields = "Id,Created,Time";
    private const string areaTitle = "Identity";

    private readonly ApplicationDbContext _context;

    public StudentListController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Identity/Student
    public IActionResult Index()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .Then("Manage StudentList");

        // Fetch descriptive data from the index dto to build the datatables index
        var metadata = DatatableExtensions.GetDtMetadata<StudentListIndexViewModel>();

        return View(metadata);
    }

    // GET: Identity/Student/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage StudentList", "Index", "StudentList", new { Area = "Identity" })
            .Then("StudentList Details");

        if (id == null)
        {
            return NotFound();
        }

        var studentList = await _context.StudentList
            .FirstOrDefaultAsync(m => m.Id == id);
        if (studentList == null)
        {
            return NotFound();
        }

        return View(studentList);
    }

    // GET: Identity/Student/Create
    public IActionResult Create()
    {
        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
            .ThenAction("Manage StudentList", "Index", "StudentList", new { Area = "Identity" })
            .Then("Create Student");

        return View();
    }

    // POST: Identity/StudentList/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(createBindingFields)] Student student)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage StudentList", "Index", "StudentListController", new { Area = "Identity" })
        .Then("Create StudentList");

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
        .ThenAction("Manage StudentList", "Index", "StudentList", new { Area = "Identity" })
        .Then("Edit StudentList");

        if (id == null)
        {
            return NotFound();
        }

        var studentList = await _context.StudentList.FindAsync(id);
        if (studentList == null)
        {
            return NotFound();
        }


        return View(studentList);
    }

    // POST: Identity/StudentList/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind(editBindingFields)] StudentList studentList)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage StudentList", "Index", "StudentList", new { Area = "Identity" })
        .Then("Edit StudentList");

        if (id != studentList.Id)
        {
            return NotFound();
        }

        StudentList model = await _context.StudentList.FindAsync(id);

        model.Created = studentList.Created;
        model.Time = studentList.Time;
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
                if (!StudentListExists(studentList.Id))
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
        return View(studentList);
    }

    // GET: Identity/Student/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        ViewData["AreaTitle"] = areaTitle;

        _breadcrumbs.StartAtAction("Dashboard", "Index", "Home", new { Area = "Dashboard" })
        .ThenAction("Manage StudentList", "Index", "StudentList", new { Area = "Identity" })
        .Then("Delete StudentList");

        if (id == null)
        {
            return NotFound();
        }

        var studentList = await _context.StudentList
            .FirstOrDefaultAsync(m => m.Id == id);
        if (studentList == null)
        {
            return NotFound();
        }

        return View(studentList);
    }

    // POST: Identity/StudentList/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var studentList = await _context.StudentList.FindAsync(id);
        _context.StudentList.Remove(studentList);
        await _context.SaveChangesAsync();

        _toast.Success("StudentList deleted successfully");

        return RedirectToAction(nameof(Index));
    }

    private bool StudentListExists(int id)
    {
        return _context.StudentList.Any(e => e.Id == id);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetStudentList(DtRequest request)
    {
        try
        {
            var query = _context.StudentList;
            var jsonData = await query.GetDatatableResponseAsync<StudentList, StudentListIndexViewModel>(request);

            return Ok(jsonData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating StudentList index json");
        }

        return StatusCode(500);
    }

}

