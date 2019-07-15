using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMS1.Models
{
    public class ActivityDocument
    {
        public int Id { get; set; }
        public string InternalName { get; set; }
        public string FileName { get; set; }
        public int ActivityId { get; set; }

        public CourseActivity Activity { get; set; }
    }
}
