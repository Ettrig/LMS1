﻿using LMS1.Data;
using LMS1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CourseModule.Include(c => c.Course);
            return View(await applicationDbContext.ToListAsync());
        }

        //// GET: CourseModules/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var courseModule = await _context.CourseModule
        //        .Include(c => c.Course)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (courseModule == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(courseModule);
        //}
   
        //GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var courseModule = await _context.CourseModule
                .Include(m => m.Activities)
                //.ThenInclude(m => m.Module)
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

            var courseModule = await _context.CourseModule.FindAsync(id);
            if (courseModule == null)
            {
                return NotFound();
            }
            // ViewData["CourseId"] = new SelectList(_context.Course, "Id", "Id", courseModule.CourseId);
            var course = await _context.Course
                .Include(c => c.Modules)
                .FirstOrDefaultAsync(c => c.Id == courseModule.CourseId);
            if (course == null)
            {
                return NotFound();
            }
            return View("Details", course);
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
                return RedirectToAction(nameof(Index));
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
            return RedirectToAction(nameof(Index));
        }

        private bool CourseModuleExists(int id)
        {
            return _context.CourseModule.Any(e => e.Id == id);
        }
    }
}
