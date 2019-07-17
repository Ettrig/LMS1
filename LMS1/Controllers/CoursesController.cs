using LMS1.Data;
using LMS1.Models;
using LMS1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CoursesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<ActionResult> StudentOrTeacher()
        {
            if (User.IsInRole("Teacher")) return RedirectToAction("Index");
            if (User.IsInRole("Student"))
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                Course course;
                if (user!=null && user.CourseId != null)
                {
                    course = await _context.Course.FirstOrDefaultAsync(c => c.Id == user.CourseId);
                    if (course!=null) return RedirectToAction("DetailsForStudent", new { id=course.Id } );
                }
            }
            return RedirectToAction("Index", "Home"); 
        }

        // GET: Courses
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Course.ToListAsync());
        }

        // GET: Courses/Details/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.CourseDocuments)
                .Include(c => c.Modules)
                .ThenInclude(c => c.Activities)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            foreach (CourseModule m in course.Modules)
                m.Activities = m.Activities
                    .OrderBy(a => a.StartDate.Date)
                    .ThenBy(a => a.EndDate).ToList();

            return View(course);
        }

        // GET: Courses/DetailsForStudent/5
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> DetailsForStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .Include(c => c.CourseDocuments)
                .Include(c => c.Modules)
                .ThenInclude(c => c.Activities)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            course.Modules = course.Modules
                .OrderBy(m => m.StartDate.Date)
                .ThenBy(m => m.EndDate)
                .ToList();

            foreach (CourseModule m in course.Modules)
                m.Activities = m.Activities
                    .OrderBy(a => a.StartDate.Date)
                    .ThenBy(a => a.EndDate).ToList();

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            int? currentModuleId = null;
            foreach (CourseModule mod in course.Modules)
            {
                foreach (CourseActivity act in mod.Activities)
                {
                    if (act.Id==user.CourseActivityId)
                    {
                        currentModuleId = act.ModuleId;
                        break; 
                    }
                }
                if (currentModuleId != null) break; 
            }

            if (user.CourseActivityId == null)
            {
                var firstModule = course.Modules.FirstOrDefault(m => true);
                if (firstModule==null) return View(new CourseForStudent() { activeModuleId = null, activeActivityId = null, course = course }); 
                currentModuleId = firstModule.Id;
                var firstActivity = firstModule.Activities.FirstOrDefault(a => true);
                if (firstActivity==null) return View(new CourseForStudent() { activeModuleId = currentModuleId, activeActivityId = null, course = course });
                user.CourseActivityId = firstActivity.Id;
                _context.Update(user);
                _context.SaveChanges();
            }

            var cfs = new CourseForStudent() { activeModuleId=currentModuleId, activeActivityId = user.CourseActivityId, course = course };

            return View(cfs);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Teacher")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Description")] Course course)
        {
            if (ModelState.IsValid)
            {
                _context.Add(course);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Description")] Course course)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var courseWithAllModules = await _context.Course
                    .Include(c => c.Modules)
                    .ThenInclude(c => c.Activities)
                    .FirstOrDefaultAsync(c => c.Id == course.Id);
                if (course == null)
                {
                    return NotFound();
                }
                return View("Details", courseWithAllModules);
            }
            return View(course);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Course
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Course
                .Include(c => c.AttendingStudents)
                .FirstOrDefaultAsync(c => c.Id==id);
            foreach (ApplicationUser student in course.AttendingStudents) {
                student.CourseId = null; // null == no course
                _context.Update(student);
            }
            _context.Course.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Courses/AddFile
        [Authorize(Roles = "Teacher")]
        public IActionResult AddFile(int? id)
        {
            if (id == null) return NotFound();
            ViewBag.CourseId = id;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(List<IFormFile> files, int courseId, string InternalName)
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
                    var docRec = new CourseDocument() { FileName = formFile.FileName, CourseId = courseId, InternalName = InternalName };
                    _context.CourseDocument.Add(docRec);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("Details", new { id = courseId });
        }

        public async Task<IActionResult> DeleteCourseFile(int? id)
        {
            if (id == null) return NotFound();

            var fil = await _context.CourseDocument.FirstOrDefaultAsync(d => d.Id == id);
            if (fil == null) return NotFound();

            //Not nice that EF reuses the name "File" in the controller
            System.IO.File.Delete("wwwroot/Documents/" + fil.FileName);

            int courseId = fil.CourseId;
            _context.CourseDocument.Remove(fil);
            _context.SaveChanges(); 

            return RedirectToAction( "Details", new { id = courseId });
        }

        // GET: Courses/AddModule
        [Authorize(Roles = "Teacher")]
        public IActionResult AddModule(int? Id)
        {
            if (Id == null) return NotFound();
            ViewBag.CourseId = Id;
            return View();
        }

        // POST: Courses/AddModule
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> AddModule([Bind("Id,Name,StartDate,EndDate,Description,CourseId")] CourseModule courseModule)
        {
            if (ModelState.IsValid)
            {
                courseModule.Id = 0;
                _context.Add(courseModule);
                await _context.SaveChangesAsync();
                var course = await _context.Course
                    .Include(c => c.Modules)   // The modules are to be seen in the course page
                    .ThenInclude(c => c.Activities)
                    .FirstOrDefaultAsync(c => c.Id == courseModule.CourseId);
                if (course == null)
                {
                    return NotFound();
                }
                return View("Details", course);
            }
            ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", courseModule.CourseId);
            return View(courseModule);
        }

        // GET: Courses/Delete/5
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteModule(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var module = await _context.CourseModule
                .FirstOrDefaultAsync(m => m.Id == id);
            if (module == null)
            {
                return NotFound();
            }
            return View(module);
        }

        // POST: Courses/DeleteModule/5
        [HttpPost, ActionName("DeleteModule")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> DeleteModuleConfirmed(int id)
        {
            var module = await _context.CourseModule.FindAsync(id);
            _context.CourseModule.Remove(module);
            await _context.SaveChangesAsync();
            var course = await _context.Course
                .Include(c => c.Modules)   // The modules are to be seen in the course page
                .FirstOrDefaultAsync(c => c.Id == module.CourseId);
            if (course == null)
            {
                return NotFound();
            }
            return View("Details", course);
        }

        private bool CourseExists(int id)
        {
            return _context.Course.Any(e => e.Id == id);
        }
    }
}
