using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using DatabaseApplication.Interfaces;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace DatabaseApplication.Pages.Registered
{
    public class LoggedInIndexModel : PageModel
    {
        private readonly UserServiceSession _userServiceSession;

        private readonly ApplicationDbContext _context;

        private readonly InventoryService _inventoryService;


        public LoggedInIndexModel(UserServiceSession userSessionService , ApplicationDbContext context, InventoryService inventoryService)
        {
            _context = context;
           // _context.Database.OpenConnection();
            _userServiceSession = userSessionService;

            //_context.Database.OpenConnectionAsync().Wait(); 
            _inventoryService = inventoryService;
        }

        public Dictionary<string, int>? ItemsByCategory { get; set; }
        public decimal TotalInventoryCost { get; set; }
        public long TotalStockIn { get; set; }
        public long TotalStockCheckedOut { get; set; }
        public int TotalCategories { get; set; }

        // Data for the pie chart
        public Dictionary<string, long>? StockData { get; set; }





        public async Task<IActionResult> OnGetAsync()
        {
            if (!_userServiceSession.IsLoggedIn)
            {
                return RedirectToPage("/Login");
            }


           
            //var testValue = _inventoryService.GetTotalStockIn();


            TotalStockIn = await _inventoryService.GetTotalStockInAsync();  
            TotalStockCheckedOut = await _inventoryService.GetTotalStockCheckedOutAsync();             
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
