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

        public int? CourseId { get; set; }
        public int? ModuleId { get; set; }
        public int? ActivityId { get; set; }
    }
}
