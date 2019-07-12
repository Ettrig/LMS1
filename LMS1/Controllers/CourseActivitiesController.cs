using LMS1.Data;
using LMS1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.Controllers
{
    public class CourseActivitiesController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CourseActivitiesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CourseActivities
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CourseActivity.Include(c => c.Module);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: CourseActivities/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseActivity = await _context.CourseActivity
                .Include(m => m.Module)
                .ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseActivity == null)
            {
                return NotFound();
            }
            return View(courseActivity);
        }

        // GET: CourseActivities/DetailsForStudent/5
        public async Task<IActionResult> DetailsForStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseActivity = await _context.CourseActivity
                .Include(m => m.Module)
                .ThenInclude(m => m.Course)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseActivity == null)
            {
                return NotFound();
            }

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
            user.CourseActivityId = courseActivity.Id;
            _context.Update(user);
            _context.SaveChanges();  

            return View(courseActivity);
        }

        // GET NextActivityForStudent/5
        public async Task<IActionResult> NextActivityForStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var thisActivity = _context.CourseActivity.FirstOrDefault(a => a.Id == id);
            if (thisActivity == null)
            {
                return NotFound();
            }

            var thisModule = _context.CourseModule
                .Include(m => m.Activities)
                .Include(m => m.Course.Modules)
                .FirstOrDefault(m => m.Id == thisActivity.ModuleId);

            if (thisModule==null) return RedirectToAction("StudentOrTeacher", "Courses");
            var sisterActivities = thisModule
                .Activities
                .OrderBy(a => a.StartDate.Date)
                .ThenBy(a => a.EndDate);

            bool foundThisActivity = false;
            int? nextActivityId = null; 
            foreach( CourseActivity a in sisterActivities)
            {
                if (foundThisActivity)
                {
                    nextActivityId = a.Id;
                    break;
                }
                if (a.Id == thisActivity.Id) foundThisActivity = true;
            }

            //Would be even better to go to next module if there is one
            if (nextActivityId == null)
            {
                bool foundThisModule = false;
                int? nextModuleId = null;
                foreach (CourseModule mod in thisModule.Course.Modules)
                {
                    if (foundThisModule)
                    {
                        nextModuleId = mod.Id;
                        break;
                    }
                    if (mod.Id == thisModule.Id) foundThisModule = true;
                }

                if (nextModuleId==null) return RedirectToAction("StudentOrTeacher", "Courses");

                var user = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);
                user.CourseActivityId = null;
                _context.Update(user);
                _context.SaveChanges();
                return RedirectToAction("DetailsForStudent", "CourseModules", new { id = nextModuleId });
            }
            return RedirectToAction("DetailsForStudent", new { id = nextActivityId }); 
        }
            
        // GET: CourseActivities/Create
        public IActionResult Create()
        {
            ViewData["ModuleId"] = new SelectList(_context.CourseModule, "Id", "Id");
            return View();
        }

        // POST: CourseActivities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,StartDate,EndDate,Description,Exercise,ModuleId")] CourseActivity courseActivity)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courseActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModuleId"] = new SelectList(_context.CourseModule, "Id", "Id", courseActivity.ModuleId);
            return View(courseActivity);
        }

        // GET: CourseActivities/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseActivity = await _context.CourseActivity.FindAsync(id);
            if (courseActivity == null)
            {
                return NotFound();
            }
            return View(courseActivity);
        }

        // POST: CourseActivities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,StartDate,EndDate,Description,Exercise,ModuleId")] CourseActivity courseActivity)
        {
            if (id != courseActivity.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courseActivity);
                    _context.Entry(courseActivity).Property(m => m.ModuleId).IsModified = false;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseActivityExists(courseActivity.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                _context.Entry(courseActivity).Reload();
                var newModuleID = _context.CourseActivity.FirstOrDefault(m => m.Id == id).ModuleId;
                if (newModuleID == 0)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Details), "CourseModules", new { id = newModuleID });
            }
            ViewData["ModuleId"] = new SelectList(_context.CourseModule, "Id", "Id", courseActivity.ModuleId);
            return View(courseActivity);
        }

        // GET: CourseActivities/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courseActivity = await _context.CourseActivity
                .Include(c => c.Module)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courseActivity == null)
            {
                return NotFound();
            }

            return View(courseActivity);
        }

        // POST: CourseActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courseActivity = await _context.CourseActivity.FindAsync(id);
            _context.CourseActivity.Remove(courseActivity);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Details), "CourseModules", new { id = courseActivity.ModuleId });
        }

        // GET: CourseActivities/BacktoModules/
        public async Task<IActionResult> BacktoModules(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var CourseActivities = await _context.CourseActivity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (CourseActivities == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(Details), "CourseModules", new { id = CourseActivities.ModuleId });
        }


        // GET: CourseActivities/StudentBacktoModules/
        public async Task<IActionResult> StudentBacktoModules(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var CourseActivities = await _context.CourseActivity
                .FirstOrDefaultAsync(m => m.Id == id);
            if (CourseActivities == null)
            {
                return NotFound();
            }
            return RedirectToAction(nameof(DetailsForStudent), "CourseModules", new { id = CourseActivities.ModuleId });
        }

        private bool CourseActivityExists(int id)
        {
            return _context.CourseActivity.Any(e => e.Id == id);
        }


    }
}
