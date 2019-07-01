using LMS1.Models;
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
        internal static void Initialize(IServiceProvider services)
        {
            var options = services.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
            using (var context = new ApplicationDbContext(options))
            {
                if (context.Course.Any())
                {
                    context.Course.RemoveRange(context.Course);
                    context.CourseModule.RemoveRange(context.CourseModule);
                }

                List<Course> courses = SeedCourses();
                context.Course.AddRange(courses);


                var courseModules = new List<CourseModule>();
                int i = 0;
                foreach (var course in courses)
                {
                    i++;
                    var courseModuleName = "M-" + i + "-" + Faker.Internet.DomainWord();
                    var courseModule = new CourseModule
                    {
                        Name = courseModuleName,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now,
                        Description = "M-" + i + "-desc",
                        CourseId = course.Id
                    };
                    courseModules.Add(courseModule);
                }
                context.CourseModule.AddRange(courseModules);


                //var courseActivity = new List<CourseActivity>();
                //int i = 0;
                //foreach (var module in modules)
                //{
                //    i++;
                //    var courseActivityName = ""
                //}


                context.SaveChanges();
            }
        }

        private static List<Course> SeedCourses()
        {
            var courses = new List<Course>();
            for (int i = 1; i <= 10; i++)
            {
                var courseName = "C-" + i + "-" + Faker.Company.CatchPhrase();
                var course = new Course
                {
                    Name = courseName,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    Description = "C-" + i + "-desc"
                };
                courses.Add(course);
            }

            return courses;
        }
    }
}
