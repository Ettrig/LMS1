using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS1.Models
{
    public class CourseModule
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Module Name")]
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

        [Display(Name = "Module Description")]
        [StringLength(750, MinimumLength = 2)]
        public string Description { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
        public ICollection<CourseActivity> Activities { get; set; }
    }
}