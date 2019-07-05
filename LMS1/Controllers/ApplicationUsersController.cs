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
                var appUser = new UserToShow();
                if (await _userManager.IsInRoleAsync(u, "Teacher")) appUser.Role = "Teacher";
                else appUser.Role = "Student";
                appUser.UserName = u.UserName;

                usersToShow.Add(appUser);
            }

            return View(usersToShow);
        }
    }
}