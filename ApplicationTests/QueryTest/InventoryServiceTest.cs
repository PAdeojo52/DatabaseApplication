using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using DatabaseApplication.NewFolder;
using DatabaseApplication.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.QueryTest
{
    public class InventoryServiceTest : IClassFixture<DatabaseTestFixture>
    {
        private readonly Supabase.Client _supabaseClient;

        public InventoryServiceTest()
        {
            _supabaseClient = new Supabase.Client(
                "https://fgplemjtxynlxftuwsit.supabase.co", // Replace with your Supabase URL
                "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImZncGxlbWp0eHlubHhmdHV3c2l0Iiwicm9sZSI6ImFub24iLCJpYXQiOjE3MzE2NDI2NDcsImV4cCI6MjA0NzIxODY0N30.lCwTE-hKQCiRksCQRzvRbjfcCzmLIhkK9VjGDmU90mA", // Replace with your Supabase API key
                new Supabase.SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true
                });
        }

        [Fact]
        public async Task InsertRandomItemsTest()
        {
            var random = new Random();

            // Define options for random fields
            var names = new[] { "Item A", "Item B", "Item C", "Item D", "Item E" };
            var descriptions = new[] { "Description A", "Description B", "Description C", "Description D", "Description E" };
            var categories = Enumerable.Range(1, 10).ToArray(); // Example category IDs (1-10)
            var prices = Enumerable.Range(10, 100).Select(i => (double)i).ToArray(); // Prices from 10 to 100
            var makes = new[] { "Make1", "Make2", "Make3", "Make4", "Make5" };
            var models = new[] { "Model1", "Model2", "Model3", "Model4", "Model5" };

            // Insert 30 random items
            for (int i = 0; i < 30; i++)
            {
                var item = new Item
                {
                    Id = random.Next(1, int.MaxValue),
                    Name = names[random.Next(names.Length)],
                    Description = descriptions[random.Next(descriptions.Length)],
                    Category = categories[random.Next(categories.Length)],
                    Price = prices[random.Next(prices.Length)],
                    Photo = $"https://example.com/photo{i}.jpg", // Example photo URL
                    Creator = null, // Creator must be null
                    ItemLocation = null, // Example: No location specified
                    Make = makes[random.Next(makes.Length)],
                    Model = models[random.Next(models.Length)],
                    CheckedIn = true, // CheckedIn must be true
                    CreatedAt = DateTime.UtcNow
                };

                // Insert the item into the database
                var response = await _supabaseClient.From<Item>().Insert(item);

                //Assert.True(response.Models.Count > 0, "Item insertion failed.");
            }
        }


        [Fact]
        public async Task AssignValidCreatorToItemsTest()
        {
            // Fetch all users from the database to use as potential creators
            var userResponse = await _supabaseClient.From<NewUser.User>().Get();
            var users = userResponse.Models;

            Assert.NotEmpty(users); // Ensure there are users in the database

            // Fetch all items from the database
            var itemResponse = await _supabaseClient.From<Item>().Get();
            var items = itemResponse.Models;

            Assert.NotNull(items); // Ensure there are items to process

            foreach (var item in items.Where(i => i.Creator == null))
            {
                // Assign a random valid user ID to the Creator field
                var random = new Random();
                var randomUser = users[random.Next(users.Count)];
                item.Creator = randomUser.Id;
                item.CheckedIn = false;

                // Update the item in the database
                var updateResponse = await _supabaseClient.From<Item>().Update(item);
                Assert.True(updateResponse.Models.Count > 0, "Failed to update item creator.");
            }
        }



        [Fact]
        public async Task SetCheckedInToFalseForAllTrueItemsTest()
        {
            // Fetch all items from the database
            var itemResponse = await _supabaseClient.From<Item>().Get();
            var items = itemResponse.Models;

            Assert.NotNull(items); // Ensure there are items to process

            foreach (var item in items.Where(i => i.CheckedIn == true))
            {
                // Set CheckedIn to false
                item.CheckedIn = false;

                // Update the item in the database
                var updateResponse = await _supabaseClient.From<Item>().Update(item);

                Assert.True(updateResponse.Models.Count > 0, $"Failed to update item with ID {item.Id}.");
            }
        }

    }


    


   
}
   
    

