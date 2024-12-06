using DatabaseApplication.EntityModels;

namespace DatabaseApplication.Services
{
    public class ItemService
    {
        private readonly Supabase.Client _supabaseClient;

        public ItemService(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            try
            {
                var response = await _supabaseClient.From<Category>().Get();

                if (response == null)
                {
                    Console.WriteLine("The response from Supabase was null.");
                    return new List<Category>();
                }

                if (response.Models == null || !response.Models.Any())
                {
                    Console.WriteLine("No categories found in the response.");
                }
                else
                {
                    Console.WriteLine($"{response.Models.Count} categories found in the response.");
                }

                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while fetching categories: {ex.Message}");
                return new List<Category>();
            }
        }

        public async Task AddItemAsync(Item newItem)
        {
            await _supabaseClient.From<Item>().Insert(newItem);
        }
    }
}
