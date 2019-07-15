using System.ComponentModel.DataAnnotations;


namespace LMS1.Models
{
    public abstract class Document
    {
        public int Id { get; set; }
        public string InternalName { get; set; }
        public string FileName { get; set; }
    }
}
