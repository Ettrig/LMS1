using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS1.Data;
using LMS1.Models;
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
            //var listOfUsers = await _context.ApplicationUser.ToListAsync();
            var listOfUsers = await _userManager.Users.ToListAsync(); 

            foreach (ApplicationUser u in listOfUsers)
            {
                bool teacher = await _userManager.IsInRoleAsync(u, "Teacher");
                if (teacher) u.Role = "Teacher";
                else u.Role = "Student";
            }

            return View(listOfUsers);
        }
    }
}