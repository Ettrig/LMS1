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


                var courses = new List<Course>();
                for (int i = 1; i <= 10; i++)
                {
                    var courseName = "C" + i + ": " + Faker.Company.CatchPhrase();
                    var course = new Course
                    {
                        Name = courseName,
                        //Email = Faker.Internet.Email(userName)
                    };
                    courses.Add(course);
                }
                

                foreach (var course in courses)
                {
                    var courseModuleName = "M";
                    var courseModule = new CourseModule
                    {
                        Name = courseModuleName,

                    };
                }

                context.Course.AddRange(courses);

                context.SaveChanges();
            }
        }
    }
}
