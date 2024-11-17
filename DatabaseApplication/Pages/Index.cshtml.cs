using DatabaseApplication.Pages.PageRouting;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        private readonly UserServiceSession _userSessionService;

        public IndexModel(UserServiceSession userSessionService)
        {
            _userSessionService = userSessionService;
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