using LMS1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.ViewModels
{
    public class CourseForStudent
    {
        public int? activeActivityId { get; set; }
        public Course course { get; set; } 
    }
}
