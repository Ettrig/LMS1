using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.ViewModels
{
    public class ClassList
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<string> Students { get; set; }
    }
}
