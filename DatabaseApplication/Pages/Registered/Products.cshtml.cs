using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages.Registered
{
    public class ProductsModel : PageModel
    {
       // private readonly ApplicationDbContext _context;

        private readonly ItemService _itemService;
        public readonly UserServiceSession _userServiceSession;
        private readonly CategoryService _categoryService;


        public ProductsModel(ItemService itemService, UserServiceSession userServiceSession, CategoryService categoryService)
        {
            //_context = context;
            _itemService = itemService;
            _userServiceSession = userServiceSession;
            _categoryService = categoryService;
        }

        // List of Items to display on the page
        public List<Item> Items { get; set; }
        public Dictionary<int, string> Categories { get; set; }

        public async Task OnGetAsync()
        {
            // Fetch all items from the Supabase Items table
            Items = await _itemService.GetAllItemsAsync();


            var categories = await _categoryService.GetAllCategoriesAsync();
            Categories = categories.ToDictionary(c => c.Id, c => c.Name);
        }

        public string GetCategoryName(int categoryId)
        {
            return Categories.TryGetValue(categoryId, out var categoryName) ? categoryName : "Unknown";
        }

        public async Task<IActionResult> OnPostCheckoutAsync(int itemId)
        {
            Items = await _itemService.GetAllItemsAsync();
            var userId = _userServiceSession.UserId;

            // Check if the user is allowed to check in the item
            var item = await _itemService.GetItemByIdAsync(itemId);

            if (item != null && item.Creator == userId)
            {
                // Perform the check-in operation
                bool successCheckIn = await _itemService.CheckInItemAsync(itemId);

                if (!successCheckIn)
                {
                    // Handle error (e.g., log it or show an error message)
                    return RedirectToPage("Error");
                }
            }
            else
            {


                // User is not authorized to check in the item
                return Unauthorized();
            }

            // Update the item in the database
            bool success = await _itemService.CheckoutItemAsync(itemId, userId);

            if (!success)
            {
                // Handle error (e.g., log it or show an error message)
                return RedirectToPage("Error");
            }

            return RedirectToPage();
        }
    }
}
