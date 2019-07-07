using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace LMS1.ViewModels
{
    public class UserToEdit
    {
        public string Id { get; set; }
        public string LmsName { get; set; }
        public string Email { get; set; }
        public int CourseId { get; set; }
        public List<SelectListItem> AllRoles { get; set; }
        public string Role { get; set; }
        public bool ChangePassword { get; set; }
        public string Password { get; set; }
    }
}
