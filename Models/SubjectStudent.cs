namespace officehours_management.Models
{
    public class SubjectStudent
    {
        public int SubjectId { get; set; }
        public Subject subject { get; set; }
        public string StudentId { get; set; }
        public ApplicationUser Student { get; set; }
    }
}