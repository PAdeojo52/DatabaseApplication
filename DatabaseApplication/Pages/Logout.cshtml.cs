using DatabaseApplication.Data;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages
{
    public class LogoutModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserServiceSession _userServiceSession;


        public LogoutModel(ApplicationDbContext context, UserServiceSession userSessionService)
        {
            _context = context;
            _userServiceSession = userSessionService;
        }
        public IActionResult OnGet()
        {
            // Clear the user session or authentication state
            //HttpContext.Session.Clear(); // Clear session if using session state
            //HttpContext.SignOutAsync(); // Clear authentication cookies (if using authentication)
            _userServiceSession.IsLoggedIn = false;
            // Redirect to the Login page or Home page
            return RedirectToPage("/Login");
        }
    }
}
