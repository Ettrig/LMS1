using System;
using System.Collections.Generic;

namespace LMS1.Models
{
    public class CourseModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public int CourseId { get; set; }

        public Course Course { get; set; }
        public ICollection<CourseActivity> Activities { get; set; }

    }
}