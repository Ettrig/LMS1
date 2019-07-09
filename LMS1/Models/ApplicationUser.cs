using Microsoft.AspNetCore.Identity;

namespace LMS1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }  // This should be removed
        public string EmailAddress { get; set; }
        public string LmsName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public int? CourseId { get; set; }

        public Course Course { get; set; }
    }
}