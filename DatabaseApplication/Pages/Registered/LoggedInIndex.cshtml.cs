using DatabaseApplication.Data;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages.Registered
{
    public class LoggedInIndexModel : PageModel
    {
        private readonly UserServiceSession _userServiceSession;

        private readonly ApplicationDbContext _context;

        public LoggedInIndexModel(UserServiceSession userSessionService , ApplicationDbContext context)
        {
            _userServiceSession = userSessionService;
            _context = context;
        }

        public Dictionary<string, int> ItemsByCategory { get; set; }
        public decimal TotalInventoryCost { get; set; }
        public long TotalStockIn { get; set; }
        public int TotalStockCheckedOut { get; set; }
        public int TotalCategories { get; set; }

        // Data for the pie chart
        public Dictionary<string, long> StockData { get; set; }



        public IActionResult OnGet()
        {
            if (!_userServiceSession.IsLoggedIn)
            {
                return RedirectToPage("/Login");
            }

            // Example logic for stock in and checked out (update with your database schema if needed)
            TotalStockIn = 10;//_context.Items.Sum(item => item.Stock);
            TotalStockCheckedOut = 10; // Replace with actual logic if available

            // Prepare chart data
            StockData = new Dictionary<string, long>
            {
                { "Stock In", TotalStockIn },
                { "Stock Checked Out", TotalStockCheckedOut }
            };

            ViewData["UserId"] = _userServiceSession.UserId; // Use the User ID as needed
            return Page();
        }
    }
}
