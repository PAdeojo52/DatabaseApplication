using DatabaseApplication.Data;
using DatabaseApplication.Interfaces;

namespace DatabaseApplication.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly ApplicationDbContext _context;

        public InventoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get total stock in from all items
        public long GetTotalStockIn()
        {
            try
            {
                return _context.Items?.Sum(item => item.Stock) ?? 1; // Default to 1 if null
            }
            catch (Exception)
            {
                return 1; // Default to 1 on exception
            }
        }

        // Placeholder for stock checked out (not yet implemented)
        public long GetTotalStockCheckedOut()
        {
            try
            {
                return _context.Items?.Sum(item => item.StockOut) ?? 1; // Default to 1 if null
            }
            catch (Exception)
            {
                return 1; // Default to 1 on exception
            }
        }

        public Dictionary<string, long> GetStockByCategory()
        {
            try
            {
                // Group by category and sum the stock for each category
                return _context.Items?
                    .GroupBy(item => item.Category)
                    .ToDictionary(
                        group => group.Key, // Category
                        group => group.Sum(item => item.Stock) // Total stock for the category
                    ) ?? new Dictionary<string, long> { { "Unknown", 0 } };
            }
            catch (Exception)
            {
                // Default to an empty dictionary on exception
                return new Dictionary<string, long> { { "Unknown", 0 } };
            }
        }
    }
}
