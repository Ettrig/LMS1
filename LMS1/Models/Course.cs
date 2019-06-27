using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }

        public ICollection<CourseModule> Modules { get; set; }
        public ICollection<ApplicationUser> AttendingStudents { get; set; }
    }
}
