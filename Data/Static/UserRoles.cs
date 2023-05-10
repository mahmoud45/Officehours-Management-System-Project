namespace officehours_management.Data.Static
{
    public class UserRoles
    {
        public const string User = "User";
        public const string Admin = "Admin";
        public const string SuperUser = "SuperUser";
        public const string Student = "Student";
        public const string TeacherAssistant = "TeacherAssistant";
        public const string Doctor = "Doctor";
        public const string AdminAndSuperUser = Admin + "," + SuperUser;
        public const string TeacherAssistantAndDoctor = TeacherAssistant + "," + Doctor;

    }
}