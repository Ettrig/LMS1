using LMS1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.OtherClasses
{
    public class StudentSubmission
    {
        public ApplicationUser student { get; set; }
        public ExerciseSubmission submission { get; set; }
    }
}
