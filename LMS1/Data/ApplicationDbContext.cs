using LMS1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LMS1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public enum UserSortState
        {
            UserAscend, CourseAscend, RoleAscend, UserDescend, CourseDescend, RoleDescend
        };

        public enum CourseUnitType
        {
            Course, Module, Activity
        }; 

        public DbSet<Course> Course { get; set; }

        public DbSet<CourseModule> CourseModule { get; set; }

        public DbSet<CourseActivity> CourseActivity { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        public DbSet<CourseDocument> CourseDocument { get; set; }

        public DbSet<ModuleDocument> ModuleDocument { get; set; }

        public DbSet<ActivityDocument> ActivityDocument { get; set; }

        public DbSet<ExerciseSubmission> ExerciseSubmission { get; set; }
    }
}
