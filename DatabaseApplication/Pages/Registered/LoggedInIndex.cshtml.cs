using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using DatabaseApplication.Interfaces;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Supabase.Interfaces;

namespace DatabaseApplication.Pages.Registered
{
    public class LoggedInIndexModel : PageModel
    {
        private readonly UserServiceSession _userServiceSession;

        private readonly ApplicationDbContext _context;

        private readonly InventoryService _inventoryService;
        private readonly ItemService _itemService;

        private readonly Supabase.Client _supabaseClient;



        public LoggedInIndexModel(UserServiceSession userSessionService , ApplicationDbContext context, InventoryService inventoryService, ItemService itemService, Supabase.Client supabaseClient)

        {
            _context = context;
            // _context.Database.OpenConnection();
            _userServiceSession = userSessionService;

            //_context.Database.OpenConnectionAsync().Wait(); 
            _inventoryService = inventoryService;
            _itemService = itemService;

            _supabaseClient = supabaseClient;

        }

        public Dictionary<string, int>? ItemsByCategory { get; set; }
        public double TotalInventoryCost { get; set; }
        public long TotalStockIn { get; set; }
        public long TotalStockCheckedOut { get; set; }
        public int TotalCategories { get; set; }
        public string? AlertMessage { get; set; } // Alert message property
        public List<Alerts> Alerts { get; set; } = new List<Alerts>();

        public List<Category> Categories { get; set; }

        public List<Category> Categories { get; set; }

        // Data for the pie chart
        public Dictionary<string, long>? StockData { get; set; }




        public Dictionary<string, long> CategoryStockData { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            if (!_userServiceSession.IsLoggedIn)
            {
                return RedirectToPage("/Login");
            }

            Categories = await _itemService.GetCategoriesAsync();


            //var testValue = _inventoryService.GetTotalStockIn();


            TotalStockIn = await _inventoryService.GetTotalStockInAsync();  
            TotalStockCheckedOut = await _inventoryService.GetTotalStockCheckedOutAsync();

            CategoryStockData = await _inventoryService.GetStockByCategoryAsync();

            TotalInventoryCost = await _inventoryService.CalculateTotalInventoryCostAsync();

            var alertsResponse = await _supabaseClient.From<Alerts>()
                .Where(alert => alert.Status == true) // Filter for active alerts
                .Get();


            Alerts = alertsResponse.Models;

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
