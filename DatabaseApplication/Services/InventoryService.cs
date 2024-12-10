using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using DatabaseApplication.Interfaces;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Supabase.Interfaces;

namespace DatabaseApplication.Services
{
    public class InventoryService 
    {
        private ApplicationDbContext _context;
        private readonly Supabase.Client _supabaseClient;

        
        public InventoryService(Supabase.Client supabaseClient)
        {
            
            _supabaseClient = supabaseClient;
        }
        public async Task<long> GetTotalStockInAsync()
        {
            var response = await _supabaseClient.From<Item>().Filter("CheckedIn", Supabase.Postgrest.Constants.Operator.Equals, "true").Get();
            return response.Models.Count; // Count of items that are CheckedIn
        }

        public async Task<long> GetTotalStockCheckedOutAsync()
        {
            var response = await _supabaseClient.From<Item>().Filter("CheckedIn", Supabase.Postgrest.Constants.Operator.Equals, "false").Get();
            return response.Models.Count; // Count of items that are not CheckedIn
        }
        public async Task<Dictionary<string, long>> GetStockByCategoryAsync()
        {
            var response = await _supabaseClient.From<Item>().Get();
            var categoriesResponse = await _supabaseClient.From<Category>().Get();

            var groupedData = response.Models
                .GroupBy(item => item.Category)
                .ToDictionary(
                    group => categoriesResponse.Models.FirstOrDefault(c => c.Id == group.Key)?.Name ?? "Unknown", // Get category name
                    group => group.LongCount() // Count the number of items in each category
                );
            return groupedData; ;
        }

        public async Task<decimal> GetTotalInventoryCostAsync()
        {
            var response = await _supabaseClient.From<Item>().Get();
            return (decimal)response.Models.Sum(item => item.Price); // Assuming 'Cost' is a property in 'Item'
        }


        public async Task<double> CalculateTotalInventoryCostAsync()
        {
            var response = await _supabaseClient.From<Item>().Get();
            return response.Models.Sum(item => item.Price); // Sum up all item prices
        }

        [Obsolete]
        public async Task<List<Alerts>> GetLowStockAlertsAsync()
        {
            var response = await _supabaseClient.From<Alerts>().Get();
            return response.Models;

            //Not Needed
        }

        // Get total stock in from all items

    }
}
