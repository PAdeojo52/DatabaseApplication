using DatabaseApplication.Services;
using DatabaseApplication.Pages.PageRouting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static DatabaseApplication.NewFolder.NewUser;

namespace DatabaseApplication.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly UserServiceSession _userServiceSession;

        public LoginModel(Supabase.Client supabaseClient, UserServiceSession userSessionService)
        {
            _supabaseClient = supabaseClient;
            _userServiceSession = userSessionService;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Step 1: Retrieve user by email from Supabase
                var userResponse = await _supabaseClient.From<User>()
                    .Filter("email", Supabase.Postgrest.Constants.Operator.Equals, Email)
                    .Single();

                var user = userResponse.Email.FirstOrDefault(); // Retrieve the single User object

                if (user == null)
                {
                    // User not found
                    ErrorMessage = "Invalid email or password.";
                    return Page();
                }

                // Step 2: Verify password
                if (!BCrypt.Net.BCrypt.Verify(Password, userResponse.HashedPassword))
                {
                    // Password does not match
                    ErrorMessage = "Invalid email or password.";
                    return Page();
                }

                // Step 3: Set user session
                _userServiceSession.UserId = userResponse.Id; // Store the user's ID in the session
                _userServiceSession.IsLoggedIn = true;

                // Step 4: Redirect to the logged-in index page
                return RedirectToPage(PageRoutes.LOGGEDINTOINDEX);
            }
            catch (Exception ex)
            {
                // Handle exceptions and provide an error message
                Console.WriteLine($"Error during login: {ex.Message}");
                ErrorMessage = "An error occurred during login. Please try again.";
                return Page();
            }
        }
    }
}