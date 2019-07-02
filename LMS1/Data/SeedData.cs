﻿using LMS1.Models;
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

                List<CourseModule> courseModules = SeedModules(context, courses);

                SeedActivities(context, courseModules);

                context.SaveChanges();
            }
        }

        private static void SeedActivities(ApplicationDbContext context, List<CourseModule> courseModules)
        {
            Random random = new Random();
            var courseActivities = new List<CourseActivity>();
            int j = 0;
            foreach (var courseModule in courseModules)
            {
                j++;
                var courseActivityName = "A" + j.ToString("D3") + "_" + Faker.Internet.DomainWord();
                var startDate = DateTime.Now.AddDays(random.Next(1, 10));
                var endDate = startDate.AddDays(random.Next(1, 10));
                var courseActivity = new CourseActivity
                {
                    Name = courseActivityName,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = "A" + j.ToString("D3") + "_desc",
                    Exercise = "E" + j.ToString("D3") + "_exercise",
                    ModuleId = courseModule.Id
                };
                courseActivities.Add(courseActivity);
            }
            context.CourseActivity.AddRange(courseActivities);
        }

        private static List<CourseModule> SeedModules(ApplicationDbContext context, List<Course> courses)
        {
            Random random = new Random();
            var courseModules = new List<CourseModule>();
            int i = 0;
            foreach (var course in courses)
            {
                i++;
                var courseModuleName = "M" + i.ToString("D3") + "_" + Faker.Internet.DomainWord();
                var startDate = DateTime.Now.AddDays(random.Next(1, 10));
                var endDate = startDate.AddDays(random.Next(1, 10));
                var courseModule = new CourseModule
                {
                    Name = courseModuleName,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = "M" + i.ToString("D3") + "_desc",
                    CourseId = course.Id
                };
                courseModules.Add(courseModule);
            }
            context.CourseModule.AddRange(courseModules);
            return courseModules;
        }

        private static List<Course> SeedCourses()
        {
            Random random = new Random();
            var courses = new List<Course>();
            for (int i = 1; i <= 10; i++)
            {
                var courseName = "C" + i.ToString("D3") + "_" + Faker.Company.CatchPhrase();
                var startDate = DateTime.Now.AddDays(random.Next(1, 10));
                var endDate = startDate.AddDays(random.Next(1, 10));
                var course = new Course
                {
                    Name = courseName,
                    StartDate = startDate,
                    EndDate = endDate,
                    Description = "C" + i.ToString("D3") + "_desc"
                };
                courses.Add(course);
            }

            return courses;
        }
    }
}
