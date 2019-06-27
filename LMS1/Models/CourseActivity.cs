using System;

namespace LMS1.Models
{
    public class CourseActivity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Exercise { get; set; }
        public int ModuleId { get; set; }

        public CourseModule Module { get; set; }
    }
}