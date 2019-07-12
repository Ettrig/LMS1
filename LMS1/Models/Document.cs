using System.ComponentModel.DataAnnotations;


namespace LMS1.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        public string FileName { get; set; }
        public string DisplayName { get; set; }
        public string Filepath { get; set; }

        // This is ugly: 3 FK, but only one is used in each instance
        // A better solution would be to declare three cross-reference tables: 
        // e.g CourseDocument
        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }

        public Course Course { get; set; }
        public CourseModule CourseModule { get; set; }
        public CourseActivity CourseActivity { get; set; }
    }
}
