using DatabaseApplication.Data;
using DatabaseApplication.Interfaces;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages.Registered
{
    public class LoggedInIndexModel : PageModel
    {
        private readonly UserServiceSession _userServiceSession;

        private readonly ApplicationDbContext _context;

        private readonly InventoryService _inventoryService;

        private readonly QueryService _queryService;

        public Dictionary<string, int>? ItemsByCategory { get; set; }
        public decimal TotalInventoryCost { get; set; }
        public long TotalStockIn { get; set; }
        public int TotalStockCheckedOut { get; set; }
        public int TotalCategories { get; set; }



        public LoggedInIndexModel(UserServiceSession userSessionService , ApplicationDbContext context, QueryService queryService)
        {
            _userServiceSession = userSessionService;
            _context = context;
            _queryService = queryService;
            _inventoryService = new InventoryService(_queryService);

            //TotalStockIn = _inventoryService.GetTotalStockIn();
        }

        

       

        // Data for the pie chart
        public Dictionary<string, long>? StockData { get; set; }




        public async Task<IActionResult> OnGetAsync()
        {
            if (!_userServiceSession.IsLoggedIn)
            {
                return RedirectToPage("/Login");
            }
            //var testvar = _inventoryService.GetTotalStockInAsync();
            // Example logic for stock in and checked out (update with your database schema if needed)
            //TotalStockIn = await _queryService.GetTotalStockInAsync();
            //StockByCategory = await _queryService.GetStockByCategoryAsync();
            //return Page();
            TotalStockIn = await _queryService.GetTotalStockInAsync();//_inventoryService.GetTotalStockIn();//_context.Items.Sum(item => item.Stock);
            TotalStockCheckedOut = 0; //_inventoryService.GetTotalStockCheckedOut(); // Replace with actual logic if available

            //Update with the Service

            // Prepare chart data
            StockData = new Dictionary<string, long>
            {
                { "Stock In", TotalStockIn },
                { "Stock Checked Out", TotalStockCheckedOut }
            };


            // StockByCategory = _inventoryService.GetStockByCategory(); //Not Fully I mplemented yet

            ViewData["UserId"] = _userServiceSession.UserId; // Use the User ID as needed
            return Page();
        }
    }
}
