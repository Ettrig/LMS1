namespace LMS1.ViewModels
{
    public class UserToEdit
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int CourseId { get; set; }
        public string RoleId { get; set; }
        public bool ChangePassword { get; set; }
        public string Password { get; set; }
    }
}
