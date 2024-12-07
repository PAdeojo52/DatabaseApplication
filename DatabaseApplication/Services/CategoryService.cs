using DatabaseApplication.EntityModels;

namespace DatabaseApplication.Services
{
    public class CategoryService
    {
        private readonly Supabase.Client _supabaseClient;

        public CategoryService(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var response = await _supabaseClient.From<Category>().Get();
            return response.Models;
        }



    }
}
