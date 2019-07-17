using LMS1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.Data
{
    public class SeedData
    {
        // target "database 'aspnet-LMS1-5EFFD6D3-9FE9-45C4-9CA5-C237D99606FC'
        const int maxCourses = 5;
        const int maxUsersRandom = 20;
        internal static async void InitializeAsync(IServiceProvider services)
        {
            var options = services.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            using (var context = new ApplicationDbContext(options))
            {
                if (context.Course.Any()) return;
                CleanDB(context);
                if (true)
                {
                    List<Course> courses = SeedCourses();
                    context.Course.AddRange(courses);
                    List<CourseModule> courseModules = SeedModules(context, courses);
                    SeedActivities(context, courseModules);
                }
                else
                {
                    List<Course> courses = CreateCourses();
                    context.Course.AddRange(courses);

                    List<CourseModule> courseModules = CreateModules(courses);
                    context.CourseModule.AddRange(courseModules);

                    List<CourseActivity> courseActivities = CreateActivities(courseModules);
                    context.CourseActivity.AddRange(courseActivities);
                }
                await context.SaveChangesAsync();
            }
        }

        private static List<CourseActivity> CreateActivities(List<CourseModule> courseModules)
        {
            var genActivities = new List<CourseActivity>();
            var title = new string[]
                { "C01M01A01", "C01M01A02" };
            var startDate = new System.DateTime[]
                { System.DateTime.Today, DateTime.Today.AddDays(1) };
            var endDate = new System.DateTime[]
                { startDate[0].AddDays(1), startDate[1].AddDays(1) };
            var description = new string[] 
                { title[0], title[1] };
            genActivities.Add(new CourseActivity
                { Name = title[0], StartDate = startDate[0], EndDate = endDate[0], Description = description[0], ModuleId = courseModules[0].Id });
            genActivities.Add(new CourseActivity
                { Name = title[1], StartDate = startDate[1], EndDate = endDate[1], Description = description[1], ModuleId = courseModules[1].Id });
            return genActivities;
        }

        private static List<CourseModule> CreateModules(List<Course> courses)
        {
            var genModules = new List<CourseModule>();
            var title = new string[]
                { "C01M01", "C01M02" };
            var startDate = new System.DateTime[]
                { courses[0].StartDate, courses[0].StartDate.AddDays(1) };
            var endDate = new System.DateTime[]
                { startDate[0].AddDays(1), startDate[1].AddDays(1) };
            var description = new string[] 
                { title[0], title[1] };
            genModules.Add(new CourseModule
                { Name = title[0], StartDate = startDate[0], EndDate = endDate[0], Description = description[0], CourseId = courses[0].Id });
            genModules.Add(new CourseModule
                { Name = title[1], StartDate = startDate[1], EndDate = endDate[1], Description = description[1], CourseId = courses[0].Id });
            return genModules;
        }

        private static List<Course> CreateCourses()
        {
            var genCourses = new List<Course>();
            var title = new string[]
                { "Java Fundamentals", "Java Intermediate", "C# Fundamentals C# 5.0 Part 1", "C# Fundamentals C# 5.0 Part 2" };
            var startDate = new System.DateTime[]
                { System.DateTime.Today.AddDays(-5), DateTime.Today.AddDays(0), DateTime.Today.AddDays(-3), DateTime.Today.AddDays(2) };
            var endDate = new System.DateTime[]
                { startDate[0].AddDays(-1), startDate[1].AddDays(4), startDate[1].AddDays(1), startDate[1].AddDays(6) };
            var description = new string[] 
                { title[0], title[1], title[2], title[3] };
            for (int i = 0; i < 4; i++)
            {
            genCourses.Add(new Course
                { Name = title[i], StartDate = startDate[i], EndDate = endDate[i], Description = description[i] });
            }
            return genCourses;
        }

        private static void CleanDB(ApplicationDbContext context)
        {
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

                var roleNames = new[] { "Teacher", "Student" }; // TODO CJA: Make these fixed roles Enums in project. Also add a 3rd Admin role.

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

                await SeedStudents(userManager);
            }
        }

        private static async Task SeedStudents(UserManager<ApplicationUser> userManager)
        {
            // TODO CJA: Avoid hardcoded courseid's
            // TODO CJA: Default password for students "Aaa111!!!" needs to be handled so that student must specify new password after first login
            const string userPassword = "Aaa111!!!";
            ApplicationUser[] usersToStore =
            {
                new ApplicationUser { LmsName = "Carl-Johan A", UserName = "carl-johana@mail.com", Email = "carl-johana@mail.com", CourseId = 1 },
                new ApplicationUser { LmsName = "Alkaka A", UserName = "alkakaa@mail.com", Email = "alkakaa@mail.com", CourseId = 2 },
                new ApplicationUser { LmsName = "Rolf E", UserName = "rolfe@mail.com", Email = "rolfe@mail.com", CourseId = 3},
                new ApplicationUser { LmsName = "Gazala A", UserName = "gazalaa@mail.com", Email = "gazalaa@mail.com", CourseId = 4 }
            };

            foreach (var userToStore in usersToStore)
            {
                var result = await userManager.CreateAsync(userToStore, userPassword);
                var addToRoleResult = await userManager.AddToRoleAsync(userToStore, "Student");
                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception(string.Join("\n", addToRoleResult.Errors));
                }
            }

            Random random = new Random();
            for (var userNum = 0; userNum < maxUsersRandom; userNum++)
            {
                var lmsName = Faker.Internet.UserName();
                var userToStore = new ApplicationUser
                {
                    LmsName = lmsName,
                    UserName = lmsName + "@mail.com",
                    Email = lmsName + "@mail.com",
                    CourseId = random.Next(1, maxCourses)
                };
                var result = await userManager.CreateAsync(userToStore, userPassword);
                var addToRoleResult = await userManager.AddToRoleAsync(userToStore, "Student");
                if (!addToRoleResult.Succeeded)
                {
                    throw new Exception(string.Join("\n", addToRoleResult.Errors));
                }
            }
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
