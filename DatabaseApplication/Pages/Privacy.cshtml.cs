using DatabaseApplication.Pages.PageRouting;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly UserServiceSession _userSessionService;
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            // Check if the user is logged in
            if (_userSessionService.IsLoggedIn)
            {
                // Render the alternate view for logged-in users
                return RedirectToPage(PageRoutes.LOGGEDINTOINDEX);
            }
            return Page();
        }
    }
}