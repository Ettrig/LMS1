using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS1.Data;
using LMS1.Models;
using LMS1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS1.Controllers
{
    public class ApplicationUsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationUsersController(ApplicationDbContext context, 
                                          UserManager<ApplicationUser> userManager,
                                          RoleManager<IdentityRole> roleManager
            )
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            List<UserToShow> usersToShow = new List<UserToShow>(); 
            var listOfUsers = await _userManager.Users.ToListAsync(); 

            foreach (ApplicationUser u in listOfUsers)
            {
                var uts = new UserToShow();

                uts.Id = u.Id; 
                uts.UserName = u.UserName;
                if (await _userManager.IsInRoleAsync(u, "Teacher")) uts.Role = "Teacher";
                else uts.Role = "Student";

                usersToShow.Add(uts);
            }

            return View(usersToShow);
        }

        public async Task<IActionResult> Details(string id)
        {
            var appUser = await _context.ApplicationUser.FindAsync(id);

            var user2Show = new UserToShow();

            user2Show.UserName = appUser.UserName;
            user2Show.Email = appUser.Email;

            var course = await _context.Course.FindAsync(appUser.CourseId);
            //user2Show.CourseName = course.Name;
            user2Show.CourseName = "Bra kurs";

            if (await _userManager.IsInRoleAsync(appUser, "Teacher")) user2Show.Role = "Teacher";
            else user2Show.Role = "Student";

            return View(user2Show);
        }
    }
}