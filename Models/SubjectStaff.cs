namespace officehours_management.Models
{
    public class SubjectStaff
    {
        public int SubjectId { get; set; }
        public Subject subject { get; set; }
        public string StaffId { get; set; }
        public ApplicationUser Staff { get; set; }
    }
}