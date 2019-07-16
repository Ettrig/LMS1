using LMS1.Models;
using LMS1.OtherClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.ViewModels
{


    public class ActivityForTeacher
    {
        public CourseActivity activity { get; set; }
        public List<StudentSubmission> studentSubmissions { get; set; }
    }
}
