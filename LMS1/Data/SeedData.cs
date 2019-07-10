using LMS1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.Data
{
    public class SeedData
    {
        static readonly int maxCourses = 4;
        internal static void Initialize(IServiceProvider services)
        {
            var options = services.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            using (var context = new ApplicationDbContext(options))
            {
                //////////////////////////   
                ///
                /// 
                /// 
                /// 
                /// 
                /// 
                /// 
                /// 
                /// 
                /// 
                /// 
                /// if (context.Course.Any()) return;
                CleanDB(context);

                List<Course> courses = SeedCourses();
                context.Course.AddRange(courses);

                List<CourseModule> courseModules = SeedModules(context, courses);

                SeedActivities(context, courseModules);

                context.SaveChanges();
            }
        }

        private static void CleanDB(ApplicationDbContext context)
        {
            // Clean DB
            context.Course.RemoveRange(context.Course);
            context.CourseModule.RemoveRange(context.CourseModule);
            context.CourseActivity.RemoveRange(context.CourseActivity);
        }

        //This is a copy of method Initialize in LexiconGym
        public static async Task InitializeRoleManagement(IServiceProvider services, string adminPW)
        {
            using (var context = new ApplicationDbContext(services.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                //Skapar en instans av UserManager och RollManager
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                if (userManager == null || roleManager == null)
                {
                    throw new Exception("UserManager or RoleManager is null");
                }

                var roleNames = new[] { "Teacher", "Student" };

                foreach (var name in roleNames)
                {
                    //Om rollen redan finns fortsätt
                    if (await roleManager.RoleExistsAsync(name)) continue;
                    //Annars skapa rollen
                    var role = new IdentityRole { Name = name };
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new Exception(string.Join("\n", result.Errors));
                    }
                }

                var emails = new[] { "adminPW@gym.se" };

                foreach (var email in emails)
                {
                    var foundUser = await userManager.FindByEmailAsync(email);
                    if (foundUser != null) continue;
                    //Skapa en ny användare
                    var user = new ApplicationUser { UserName = email, Email = email, LmsName = "SuperTeacher", CourseId = null }; // CourseId==null
                    var addUserResult = await userManager.CreateAsync(user, adminPW);
                    if (!addUserResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addUserResult.Errors));
                    }
                }

                await SeedStudents(userManager);

                var adminUser = await userManager.FindByEmailAsync(emails[0]);
                foreach (var role in roleNames)
                {
                    //Lägg till alla roller till adminUser
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, role);
                    if (!addToRoleResult.Succeeded)
                    {
                        throw new Exception(string.Join("\n", addToRoleResult.Errors));
                    }
                }
            }
        }

        private static async Task SeedStudents(UserManager<ApplicationUser> userManager)
        {
            // TODO: Add course to all new student seeds
            //var userToStore = new ApplicationUser { LmsName = user.LmsName, UserName = user.Email, Email = user.Email, CourseId = user.CourseId };
            var userToStore = new ApplicationUser { LmsName = "Carl-Johan A", UserName = "carl-johana@mail.com", Email = "carl-johana@mail.com", CourseId = null };
            var result2 = await userManager.CreateAsync(userToStore, "Aaa111!!!" /*user.PasswordHash*/);
            var resultAddRole = await userManager.AddToRoleAsync(userToStore, "Student");

            userToStore = new ApplicationUser { LmsName = "Alkaka A", UserName = "alkaka@mail.com", Email = "alkaka@mail.com", CourseId = null };
            result2 = await userManager.CreateAsync(userToStore, "Aaa111!!!" /*user.PasswordHash*/);
            resultAddRole = await userManager.AddToRoleAsync(userToStore, "Student");

            userToStore = new ApplicationUser { LmsName = "Rolf E", UserName = "rolfe@mail.com", Email = "rolfe@mail.com", CourseId = null };
            result2 = await userManager.CreateAsync(userToStore, "Aaa111!!!" /*user.PasswordHash*/);
            resultAddRole = await userManager.AddToRoleAsync(userToStore, "Student");
        }

        private static void SeedActivities(ApplicationDbContext context, List<CourseModule> courseModules)
        {
            Random random = new Random();
            var courseActivities = new List<CourseActivity>();
            int maxCourseActivities = 0;
            foreach (var courseModule in courseModules)
            {
                int j = 0;
                maxCourseActivities++;
                for (int tempCourseActivity = 1; tempCourseActivity <= maxCourseActivities; tempCourseActivity++)
                {
                    j++;
                    var courseActivityName = "A" + j.ToString("D2") + "_" + Faker.Internet.DomainWord();
                    var startDate = DateTime.Now.AddDays(random.Next(1, 10));
                    var endDate = startDate.AddDays(random.Next(1, 10));
                    var courseActivity = new CourseActivity
                    {
                        Name = courseActivityName,
                        StartDate = startDate,
                        EndDate = endDate,
                        Description = "A" + j.ToString("D2") + "_Description",
                        Exercise = "E" + j.ToString("D2") + "_Description",
                        ModuleId = courseModule.Id
                    };
                    courseActivities.Add(courseActivity);
                }
            }
            context.CourseActivity.AddRange(courseActivities);
        }

        private static List<CourseModule> SeedModules(ApplicationDbContext context, List<Course> courses)
        {
            Random random = new Random();
            var courseModules = new List<CourseModule>();
            int maxCourseModules = -1;
            foreach (var course in courses)
            {
                int i = 0;
                maxCourseModules++;
                for (int tempCourseModule = 1; tempCourseModule <= maxCourseModules; tempCourseModule++)
                {
                    i++;
                    var courseModuleName = "M" + i.ToString("D2") + "_" + Faker.Internet.DomainWord();
                    var startDate = DateTime.Now.AddDays(random.Next(1, 10));
                    var endDate = startDate.AddDays(random.Next(1, 10));
                    var courseModule = new CourseModule
                    {
                        Name = courseModuleName,
                        StartDate = startDate,
                        EndDate = endDate,
                        Description = "M" + i.ToString("D2") + "_Description",
                        CourseId = course.Id
                    };
                    courseModules.Add(courseModule);
                }
            }
            context.CourseModule.AddRange(courseModules);
            return courseModules;
        }

        private static List<Course> SeedCourses()
        {
            Random random = new Random();
            var courses = new List<Course>();
            for (int i = 1; i <= maxCourses; i++)
            {
                var courseName = "C" + i.ToString("D2") + "_" + Faker.Company.CatchPhrase();
                var startDate = DateTime.Now.AddDays(random.Next(1, 10));
                var endDate = startDate.AddDays(random.Next(1, 10));
                var course = new Course
                {
                    Name = courseName,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = "C" + i.ToString("D2") + "_Description"
                };
                courses.Add(course);
            }
            return courses;
        }
    }
}
