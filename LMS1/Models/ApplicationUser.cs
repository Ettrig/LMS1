using Microsoft.AspNetCore.Identity;

namespace LMS1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string LmsName { get; set; }
        public int? CourseId { get; set; }
        public int? CourseActivityId { get; set; } // Last visited Activity

        public Course Course { get; set; }
        public CourseActivity CourseActivity { get; set; }
    }
}
