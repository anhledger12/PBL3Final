namespace PBL3.Data
{
    public class UserRole
    {
        public const string Admin = "Admin";
        public const string Staff = "Staff";
        public const string User = "User";
        public const string AdminOrStaff = Admin + "," + Staff;
        public const string All = Admin + "," + User + "," +Staff;
    }
}
