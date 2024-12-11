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
        public async Task<List<Item>> GetAllItemsAsync()
        {
            var response = await _supabaseClient.From<Item>().Get();
            return response.Models;
        }

        public async Task<bool> CheckoutItemAsync(int itemId, int userId)
        {
            try
            {
                // Fetch the item to update
                var itemResponse = await _supabaseClient.From<Item>().Filter("id", Supabase.Postgrest.Constants.Operator.Equals, itemId).Get();
                var item = itemResponse.Models.FirstOrDefault();

                if (item == null)
                {
                    return false; // Item not found
                }

                // Update the item's CheckedIn and Borrower fields
                item.CheckedIn = false;
                item.Creator = userId;

                // Update the item in the database
                var response = await _supabaseClient.From<Item>().Update(item);

                return response.Models.Count > 0; // Return true if update was successful
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking out item: {ex.Message}");
                return false;
            }
        }



        public async Task<Item?> GetItemByIdAsync(int itemId)
        {
            var response = await _supabaseClient.From<Item>().Filter("id", Supabase.Postgrest.Constants.Operator.Equals, itemId).Single();
            return response;
        }

        public async Task<bool> CheckoutItemSecondaryAsync(int itemId, int userId)
        {
            var response = await _supabaseClient
                .From<Item>()
                .Where(x => x.Id == itemId)
                .Set(x => x.CheckedIn, false)
                .Set(x => x.Creator, userId)
                .Update();

            return response != null;
        }

        public async Task<bool> CheckInItemAsync(int itemId)
        {
            try
            {

                // Use the latest Supabase syntax to update specific fields
                var response = await _supabaseClient
                    .From<Item>()
                    .Where(x => x.Id == itemId) // Filter by the item ID
                    .Set(x => x.CheckedIn, true) // Update the CheckedIn field
                    .Update(); // Execute the update operation

                // Check if the update was successful
                return response.Models != null && response.Models.Any();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking in item: {ex.Message}");
                return false;
            }
        }


    }
}
