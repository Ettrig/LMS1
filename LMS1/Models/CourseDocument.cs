using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LMS1.Data.ApplicationDbContext;

namespace LMS1.Models
{
    public class CourseDocument : Document
    {
        public int CourseId { get; set; }
    }
}
