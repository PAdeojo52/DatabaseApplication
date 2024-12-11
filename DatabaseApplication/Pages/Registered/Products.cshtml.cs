using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages.Registered
{
    public class ProductsModel : PageModel
    {
<
        private readonly ItemService _itemService;
        public readonly UserServiceSession _userServiceSession;
        private readonly CategoryService _categoryService;

        public ProductsModel(ItemService itemService, UserServiceSession userServiceSession, CategoryService categoryService)
        {

            _itemService = itemService;
            _userServiceSession = userServiceSession;
            _categoryService = categoryService;
        }

        // List of Items to display on the page
        public List<Item> Items { get; set; }
        public Dictionary<int, string> Categories { get; set; }


        public async Task OnGetAsync(string? categoryFilter, string? availabilityFilter)
        {
            // Fetch all items from the Supabase Items table
            var allItems = await _itemService.GetAllItemsAsync();

            // Filter by category
            if (!string.IsNullOrEmpty(categoryFilter) && int.TryParse(categoryFilter, out int categoryId))
            {
                allItems = allItems.Where(item => item.Category == categoryId).ToList();
            }

            // Filter by availability
            if (!string.IsNullOrEmpty(availabilityFilter) && bool.TryParse(availabilityFilter, out bool isAvailable))
            {
                allItems = allItems.Where(item => item.CheckedIn == isAvailable).ToList();
            }

            Items = allItems;

            // Fetch categories for the dropdown
            var categories = await _categoryService.GetAllCategoriesAsync();
            Categories = categories.ToDictionary(c => c.Id, c => c.Name);
        }

        public string GetCategoryName(int categoryId)
        {
            return Categories.TryGetValue(categoryId, out var categoryName) ? categoryName : "Unknown";

        }

        public async Task<IActionResult> OnPostCheckoutAsync(int itemId)
        {
            var userId = _userServiceSession.UserId;

            // Fetch the item by ID
            var item = await _itemService.GetItemByIdAsync(itemId);
            if (item == null)
            {
                // If the item does not exist, return an error
                return NotFound();
            }

            if (!item.CheckedIn.HasValue || !item.CheckedIn.Value)
            {
                // If the item is already checked out, return an error
                ModelState.AddModelError("", "Item is already checked out.");
                return RedirectToPage();
            }

            // Perform the checkout operation
            bool success = await _itemService.CheckoutItemAsync(itemId, userId);

            if (!success)
            {
                // Handle the error (e.g., log it or show an error message)
                return RedirectToPage("Error");
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCheckInAsync(int itemId)
        {
            var userId = _userServiceSession.UserId;

            // Fetch the item by ID
            var item = await _itemService.GetItemByIdAsync(itemId);
            if (item == null)
            {
                // If the item does not exist, return an error
                return NotFound();
            }

            if (item.CheckedIn.HasValue && item.CheckedIn.Value)
            {
                // If the item is already checked in, return an error
                ModelState.AddModelError("", "Item is already checked in.");
                return RedirectToPage();
            }

            if (item.Creator != userId)
            {
                // If the item was checked out by another user, deny access
                return Forbid();
            }

            // Perform the check-in operation
            bool success = await _itemService.CheckInItemAsync(itemId);

            if (!success)
            {
                // Handle the error (e.g., log it or show an error message)
                return RedirectToPage("Error");
            }

            return RedirectToPage();
        }



    }
}
