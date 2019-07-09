﻿using System.ComponentModel.DataAnnotations;

namespace LMS1.ViewModels
{
    public class UserToShow
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Name")]
        [StringLength(50, MinimumLength = 2)]
        public string LmsName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address.")]
        public string Email { get; set; }

        public string Role { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        [StringLength(100, MinimumLength = 2)]
        public string CourseName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "Second Name")]
        [StringLength(50, MinimumLength = 2)]
        public string SecondName { get; set; }
    }
}
