using LMS1.Data;
using LMS1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace LMS1.Controllers
{
    public class CourseModulesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourseModulesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CourseModules
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CourseModule.Include(c => c.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        //GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseModule = await _context.CourseModule
                .Include(m => m.Activities)
                .Include(m => m.Course)
                .Include(c => c.ModuleDocuments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseModule == null)
            {
                return NotFound();
            }
            return View(courseModule);
        }

        //GET: Courses/Details/5
        public async Task<IActionResult> DetailsForStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseModule = await _context.CourseModule
                .Include(m => m.Activities)
                .Include(m => m.Course)
                .Include(c => c.ModuleDocuments)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseModule == null)
            {
                return NotFound();
            }
            return View(courseModule);
        }

        // GET: CourseModules/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id");
            return View();
        }

        // POST: CourseModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Description,CourseId")] CourseModule courseModule)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseModule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", courseModule.CourseId);
            return View(courseModule);
        }

        // GET: CourseModules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.CourseModule.FindAsync(id);
            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }

        // POST: CourseModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Description,CourseId")] CourseModule courseModule)
        {
            if (id != courseModule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseModule);
                    _context.Entry(courseModule).Property(m => m.CourseId).IsModified = false; //Leave CourseId as it is in the DB
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseModuleExists(courseModule.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _context.Entry(courseModule).Reload();
                // _context.Entry(courseModule).Property(m => m.CourseId).IsModified = true; //Can this let me get proper CourseId?
                var newModuleID = _context.CourseModule.FirstOrDefault(m => m.Id == id).CourseId;
                //await _context.CourseModule.FindAsync(id);
                if (newModuleID == 0)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Details), "Courses", new { id = newModuleID });
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", courseModule.CourseId);
            return View(courseModule);
        }


        // GET: CourseModules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseModule = await _context.CourseModule
                .Include(c => c.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseModule == null)
            {
                return NotFound();
            }

            return View(courseModule);
        }

        // POST: CourseModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseModule = await _context.CourseModule.FindAsync(id);
            _context.CourseModule.Remove(courseModule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), "Courses", new { id = courseModule.CourseId });
        }

        private bool CourseModuleExists(int id)
        {
            return _context.CourseModule.Any(e => e.Id == id);
        }

        // GET: Courses/AddFile
        [Authorize(Roles = "Teacher")]
        public IActionResult AddFile(int? id)
        {
            if (id == null) return NotFound();
            ViewBag.ModuleId = id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(List<IFormFile> files, int moduleId, string InternalName)
        {
            // We handle only one file at a time, so this foreach should not be needed. 
            // Maybe just take the first item in the list. 
            // Maybe change the .cshtml so that it does not return a list. 
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(
                        "wwwroot/Documents/" + formFile.FileName,
                        FileMode.Create)
                    )
                    {
                        await formFile.CopyToAsync(stream);
                    }
                    var docRec = new ModuleDocument() { FileName = formFile.FileName, ModuleId = moduleId, InternalName = InternalName };
                    _context.ModuleDocument.Add(docRec);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Details", new { id = moduleId });
        }

        public async Task<IActionResult> DeleteModuleFile(int? id)
        {
            if (id == null) return NotFound();

            var fil = await _context.ModuleDocument.FirstOrDefaultAsync(d => d.Id == id);
            if (fil == null) return NotFound();

            //Not nice that EF reuses the name "File" in the controller
            System.IO.File.Delete("wwwroot/Documents/" + fil.FileName);

            int moduleId = fil.ModuleId;
            _context.ModuleDocument.Remove(fil);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = moduleId });
        }

        // GET: Courses/AddActivity
        public IActionResult AddActivity(int? Id)
        {
            if (Id == null) return NotFound();
            ViewBag.ModuleId = Id;
            return View();
        }

        // POST: Courses/AddActivity
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivity([Bind("Id,Name,StartDate,EndDate,Description,Exercise,ModuleId")] CourseActivity courseActivity)
        {
            if (ModelState.IsValid)
            {
                courseActivity.Id = 0;
                _context.Add(courseActivity);
                await _context.SaveChangesAsync();
                //var courseModule = await _context.Course
                //    .Include(c => c.Modules)
                //    .FirstOrDefaultAsync(m => m.Id == courseActivity.ModuleId);
                //if (courseModule == null)
                //{
                //    return NotFound();
                //}
                //return View("Details", courseModule);
                return RedirectToAction(nameof(Details), new { id = courseActivity.ModuleId });
            }
            ViewData["ModuleId"] = new SelectList(_context.Course, "Id", "Id", courseActivity.ModuleId);
            return View(courseActivity);
        }

        // GET: CourseModules/BacktoCourse/
        public async Task<IActionResult> BacktoCourse(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseModule = await _context.CourseModule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseModule == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Details), "Courses", new { id = courseModule.CourseId });
        }

        // GET: CourseModules/StudentBacktoModules/
        public async Task<IActionResult> StudentBacktoModules(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseModule = await _context.CourseModule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseModule == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(DetailsForStudent), "Courses", new { id = courseModule.CourseId });
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}


