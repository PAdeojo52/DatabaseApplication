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


            // Step 1: Retrieve user by email
            var user = await _context.Users.SingleOrDefaultAsync(u => u.email == Email);
            if (user == null)
            {
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            // Fetch the user from the database
            //var user = await _context.Users.SingleOrDefaultAsync(u => u.email == Email);

            String hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

            if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.hashed_password))
            {
                // Authentication failed
                ErrorMessage = "Invalid email or password.";
                return Page();
            }

            _userServiceSession.UserId = user.id;
            _userServiceSession.IsLoggedIn = true;

            // Authentication successful
            // TODO: Set up user session or redirect to another page
            return RedirectToPage(PageRoutes.LOGGEDINTOINDEX);
        }
    }
 }
