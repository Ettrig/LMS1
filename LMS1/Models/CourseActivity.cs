using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS1.Models
{
    public class CourseActivity
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [Display(Name = "Activity Name")]
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

        [Display(Name = "Activity Description")]
        [StringLength(5000, MinimumLength = 2)]
        public string Description { get; set; }

        [Display(Name = "Exercise Text")]
        [StringLength(2500, MinimumLength = 2)]
        public string Exercise { get; set; }


        public int ModuleId { get; set; }
        public CourseModule Module { get; set; }

        public ICollection<ActivityDocument> ActivityDocuments { get; set; }
        public ICollection<ExerciseSubmission> Submissions { get; set; }

    }
}


