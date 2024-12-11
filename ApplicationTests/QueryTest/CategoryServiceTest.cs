using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationTests.QueryTest
{
    public class CategoryServiceTest
    {

        private const string SupabaseUrl = "https://your-supabase-url.supabase.co";
        private const string SupabaseKey = "your-supabase-key";
        private readonly Client _supabaseClient;

        public CategoryServiceTest()
        {
            // Initialize the real Supabase client
            _supabaseClient = new Client(SupabaseUrl, SupabaseKey);
            _supabaseClient.InitializeAsync().Wait(); // Ensure the client is initialized before running tests
        }

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnListOfCategories()
        {
            // Arrange
            var categoryService = new CategoryService(_supabaseClient);

            // Act
            List<Category> result = await categoryService.GetAllCategoriesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 0, "Expected at least one category in the database.");
        }


    }
}
