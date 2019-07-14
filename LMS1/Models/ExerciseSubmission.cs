using System;
using System.ComponentModel.DataAnnotations;

namespace LMS1.Models
{
    public class ExerciseSubmission
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public DateTime SubmissionTime { get; set; }
        public string ApplicationUserId { get; set; }
        public int CourseActivityId { get; set; }

        public ApplicationUser User { get; set; }
        public CourseActivity Activity { get; set; }
    }
}
