using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatabaseApplication.Data;


namespace DatabaseApplication.Services
{
    public class QueryService
    {
        private readonly ApplicationDbContext _context;
        private long staticNumber = 1;

        public QueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<long> GetTotalStockInAsync()
        {
            try
            {
                // Use FromSqlRaw to execute raw SQL
                var totalStockIn = await _context.Items
                    .FromSqlRaw("SELECT SUM(\"stock\") AS stock FROM \"Items\"")
                    .Select(i => i.Stock) // Map to the Stock property
                    .FirstOrDefaultAsync();

                return totalStockIn; // Default to 1 if null
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in GetTotalStockInAsync: {ex.Message}");
                return 1; // Default to 1 on exception
            }
        }

        public async Task<long> GetTotalStockCheckedOutAsync()
        {
            try
            {
                await _context.Database.OpenConnectionAsync(); // Explicitly open the connection

                var command = _context.Database.GetDbConnection().CreateCommand();
                command.CommandText = "SELECT COALESCE(SUM(\"stock\"), 0) AS stock FROM \"Items\"";

                var result = await command.ExecuteScalarAsync(); // Execute the scalar query
                return result != DBNull.Value ? Convert.ToInt64(result) : 1; // Return the result or 1 if null
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTotalStockInAsync: {ex.Message}");
                return 1; // Default to 1 on exception
            }
            finally
            {
                await _context.Database.CloseConnectionAsync(); // Ensure the connection is closed
            }
        }

        public async Task<Dictionary<string, long>> GetStockByCategoryAsync()
        {
            try
            {
                return await _context.Items
                    .GroupBy(item => item.Category)
                    .ToDictionaryAsync(
                        group => group.Key, // Category
                        group => group.Sum(item => item.Stock) // Total stock for the category
                    );
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in GetStockByCategoryAsync: {ex.Message}");
                return new Dictionary<string, long> { { "Unknown", 0 } }; // Default on error
            }
        }
    }
}
