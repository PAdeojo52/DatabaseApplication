using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages.Registered
{
    public class LoggedInIndexModel : PageModel
    {
        private readonly UserServiceSession _userServiceSession;

        public LoggedInIndexModel(UserServiceSession userSessionService)
        {
            _userServiceSession = userSessionService;
        }
        public IActionResult OnGet()
        {
            if (!_userServiceSession.IsLoggedIn)
            {
                return RedirectToPage("/Login");
            }

            ViewData["UserId"] = _userServiceSession.UserId; // Use the User ID as needed
            return Page();
        }
    }
}
