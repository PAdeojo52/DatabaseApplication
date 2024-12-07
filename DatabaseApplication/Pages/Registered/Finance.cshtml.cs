using DatabaseApplication.EntityModels;
using DatabaseApplication.NewFolder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages.Registered
{
    public class FinanceModel : PageModel
    {
        private readonly Supabase.Client _supabaseClient;

        public FinanceModel(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public double TotalInventoryValue { get; set; }
        public int UsersWithCheckedOutItems { get; set; }
        public Dictionary<string, double> InventoryValueByCategory { get; set; }
        public Dictionary<string, double> MostValuableItems { get; set; }
        public Dictionary<string, double> UserCheckoutValue { get; set; }
        public Dictionary<string, int> CheckoutsByCategory { get; set; }
        public Dictionary<string, int> TopBorrowers { get; set; }
        public Dictionary<DateTime, double> RevenueProjection { get; set; }
        public Dictionary<string, int> OverdueItems { get; set; }
        public Dictionary<string, int> LowStockItems { get; set; }
        public double AverageCheckoutValue { get; set; }
        public Dictionary<string, (double InventoryValue, int Checkouts)> FinancialImpactByCategory { get; set; }
        public (int UsersWithCheckouts, int UsersWithoutCheckouts) UserCheckoutSplit { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch all items
            var itemsResponse = await _supabaseClient.From<Item>().Get();
            var items = itemsResponse.Models;


            var usersResponse = await _supabaseClient.From<NewUser.User>().Get();

            var users = usersResponse.Models;

            InventoryValueByCategory = items
                .GroupBy(item => item.Category)
                .ToDictionary(
                    group => $"Category {group.Key}",
                    group => group.Sum(item => item.Price)
                );

            MostValuableItems = items
                .OrderByDescending(item => item.Price)
                .GroupBy(item => item.Name ?? "Unknown") // Group by name to handle duplicates
                .Take(5) // Take top 5 groups
                .ToDictionary(
                    group => group.Key,
                    group => group.Max(item => item.Price) // Take the maximum price for duplicates
                );

            // User Checkout Value
            UserCheckoutValue = items
                .Where(item => item.Creator != null)
                .GroupBy(item => users.FirstOrDefault(u => u.Id == item.Creator)?.Email ?? "Unknown")
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(item => item.Price)
                );

            // Checkouts by Category
            CheckoutsByCategory = items
                .Where(item => item.Creator != null)
                .GroupBy(item => item.Category)
                .ToDictionary(
                    group => $"Category {group.Key}",
                    group => group.Count()
                );
            // Top Borrowers
            TopBorrowers = items
                .Where(item => item.Creator != null)
                .GroupBy(item => users.FirstOrDefault(u => u.Id == item.Creator)?.Email ?? "Unknown")
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                );

            // 10. Revenue Projection (Line Chart)
            RevenueProjection = items
                .GroupBy(item => item.CreatedAt.Date)
                .ToDictionary(
                    group => group.Key,
                    group => group.Sum(item => item.Price)
                );

            // 11. Overdue Items (Bar Chart)
            // For demonstration, assume overdue items are based on a fixed condition (e.g., 30 days since created).
            var overdueThreshold = DateTime.UtcNow.AddDays(-30);
            OverdueItems = items
                .Where(item => item.Creator != null && item.CreatedAt < overdueThreshold)
                .GroupBy(item => users.FirstOrDefault(u => u.Id == item.Creator)?.Email ?? "Unknown")
                .ToDictionary(
                    group => group.Key,
                    group => group.Count()
                );

            // 12. Low Stock Items (Bar Chart)
            // Assume low stock is represented by a column in the Item table, e.g., `Stock`.
            

            // 13. Average Checkout Value (Gauge/Single Value Display)
            var checkedOutItems = items.Where(item => item.Creator != null).ToList();
            AverageCheckoutValue = checkedOutItems.Any()
                ? checkedOutItems.Sum(item => item.Price) / checkedOutItems.Count
                : 0;

            // 14. Financial Impact of Each Category (Radar Chart)
            FinancialImpactByCategory = items
                .GroupBy(item => item.Category)
                .ToDictionary(
                    group => $"Category {group.Key}",
                    group => (
                        InventoryValue: group.Sum(item => item.Price),
                        Checkouts: group.Count(item => item.Creator != null)
                    )
                );

            // 15. Users With No Checkouts (Pie Chart)
            var usersWithCheckouts = items.Select(item => item.Creator).Distinct().Count(id => id != null);
            UserCheckoutSplit = (
                UsersWithCheckouts: usersWithCheckouts,
                UsersWithoutCheckouts: users.Count - usersWithCheckouts
            );


            // Calculate total inventory value
            TotalInventoryValue = items.Sum(item => item.Price);

            // Count users with checked-out items
            UsersWithCheckedOutItems = items.Count(item => item.Creator != null);
        }
    }
}
