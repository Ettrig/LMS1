using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS1.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Course Name")]
        [StringLength(70, MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Course Description")]
        [StringLength(750, MinimumLength = 2)]
        public string Description { get; set; }


        public ICollection<CourseModule> Modules { get; set; }
        public ICollection<ApplicationUser> AttendingStudents { get; set; }
    }
}




//        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]