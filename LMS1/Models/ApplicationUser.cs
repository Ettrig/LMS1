using Microsoft.AspNetCore.Identity;

namespace LMS1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }  // This must be handled better, to support restrictions by role to actions
                                          // Maybe we can use roles that are defined for IdentityUser
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string EmailAddress { get; set; }
        public int? CourseId { get; set; }

        public Course Course { get; set; }
    }
}