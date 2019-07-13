using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.Models
{
    public class CourseDocument
    {
        public int Id { get; set; }
        public string InternalName { get; set; }
        public string FileName { get; set; }
        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
