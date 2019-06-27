using Microsoft.AspNetCore.Identity;

namespace LMS1.Models
{
    public class ApplicationUser : IdentityUser
    {
        // public override string Id { get; set; }
        public string Role { get; set; }  // This must be handled better, to support restrictions by role to actions
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string EmailAddress { get; set; }
        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}