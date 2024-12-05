using DatabaseApplication.Data;
using DatabaseApplication.Pages.PageRouting;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;

namespace DatabaseApplication.Pages
{
    public class LoginModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserServiceSession _userServiceSession;


        public LoginModel(ApplicationDbContext context, UserServiceSession userSessionService)
        {
            _context = context;
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
                // Open the database connection explicitly
                await _context.Database.OpenConnectionAsync();

                // Step 1: Retrieve user by email
                var user = await _context.Users.SingleOrDefaultAsync(u => u.email == Email);
                if (user == null)
                {
                    ErrorMessage = "Invalid email or password.";
                    return Page();
                }

                // Verify the password
                if (!BCrypt.Net.BCrypt.Verify(Password, user.hashed_password))
                {
                    // Authentication failed
                    ErrorMessage = "Invalid email or password.";
                    return Page();
                }

                // Set up the user session
                _userServiceSession.UserId = user.id;
                _userServiceSession.IsLoggedIn = true;

                // Authentication successful
                return RedirectToPage(PageRoutes.LOGGEDINTOINDEX);
            }
            catch (Exception ex)
            {
                // Log the error for debugging purposes
                Console.WriteLine($"Error in OnPostAsync: {ex.Message}");
                ErrorMessage = "An error occurred during authentication. Please try again later.";
                return Page();
            }
            finally
            {
                // Ensure the database connection is closed
                await _context.Database.CloseConnectionAsync();
            }
        }
    }
 }
