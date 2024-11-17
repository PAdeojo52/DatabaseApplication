namespace DatabaseApplication.Services
{
    public class UserServiceSession
    {
        public bool IsLoggedIn { get; set; } = false;
        public int UserId { get; set; } // Holds the logged-in user's ID
    }
}
