using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.QueryTest
{
    public class ItemServiceIntegrationTests
    {
        private const string SupabaseUrl = "https://your-supabase-url.supabase.co"; // Replace with your Supabase URL
        private const string SupabaseKey = "your-supabase-key"; // Replace with your Supabase API key

        private readonly Client _supabaseClient;

        public ItemServiceIntegrationTests()
        {
            // Initialize the Supabase client
            _supabaseClient = new Client(SupabaseUrl, SupabaseKey);
            _supabaseClient.InitializeAsync().Wait(); // Ensure initialization before running the tests
        }

        [Fact]
        public async Task GetCategoriesAsync_ShouldReturnCategories()
        {
            // Arrange
            var itemService = new ItemService(_supabaseClient);

            // Act
            var categories = await itemService.GetCategoriesAsync();

            // Assert
            Assert.NotNull(categories);
            Assert.True(categories.Count > 0, "Expected at least one category in the database.");
        }

        [Fact]
        public async Task AddItemAsync_ShouldAddNewItem()
        {
            // Arrange
            var itemService = new ItemService(_supabaseClient);
            var newItem = new Item
            {
                Name = "Test Item",
                CheckedIn = true,
                Creator = 1 // Replace with a valid user ID
            };

            // Act
            await itemService.AddItemAsync(newItem);

            // Assert
            var allItems = await itemService.GetAllItemsAsync();
            Assert.Contains(allItems, i => i.Name == "Test Item");
        }

        [Fact]
        public async Task CheckoutItemAsync_ShouldUpdateCheckedInAndCreator()
        {
            // Arrange
            var itemService = new ItemService(_supabaseClient);
            var newItem = new Item
            {
                Name = "Item to Checkout",
                CheckedIn = true,
                Creator = 0
            };
            await itemService.AddItemAsync(newItem);
            var allItems = await itemService.GetAllItemsAsync();
            var itemToCheckout = allItems.FirstOrDefault(i => i.Name == "Item to Checkout");

            Assert.NotNull(itemToCheckout); // Ensure the item exists
            int itemId = (int)itemToCheckout.Id;

            // Act
            bool checkoutResult = await itemService.CheckoutItemAsync(itemId, 1); // Replace with a valid user ID

            // Assert
            Assert.True(checkoutResult);

            var updatedItem = await itemService.GetItemByIdAsync(itemId);
            Assert.NotNull(updatedItem);
            Assert.False(updatedItem.CheckedIn);
            Assert.Equal(1, updatedItem.Creator);
        }

        [Fact]
        public async Task CheckInItemAsync_ShouldUpdateCheckedInToTrue()
        {
            // Arrange
            var itemService = new ItemService(_supabaseClient);
            var newItem = new Item
            {
                Name = "Item to CheckIn",
                CheckedIn = false,
                Creator = 1
            };
            await itemService.AddItemAsync(newItem);
            var allItems = await itemService.GetAllItemsAsync();
            var itemToCheckIn = allItems.FirstOrDefault(i => i.Name == "Item to CheckIn");

            Assert.NotNull(itemToCheckIn); // Ensure the item exists
            int itemId = (int)itemToCheckIn.Id;

            // Act
            bool checkInResult = await itemService.CheckInItemAsync(itemId);

            // Assert
            Assert.True(checkInResult);

            var updatedItem = await itemService.GetItemByIdAsync(itemId);
            Assert.NotNull(updatedItem);
            Assert.True(updatedItem.CheckedIn);
        }
    }
}
