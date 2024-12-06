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

        // Get total stock in from all items

    }
}
