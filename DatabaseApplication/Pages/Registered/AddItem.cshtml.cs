using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace DatabaseApplication.Pages.Registered
{
    public class AddItemModel : PageModel
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly ItemService _itemService;
        private readonly UserServiceSession _userService;
        public AddItemModel(Supabase.Client supabaseClient, ItemService itemService, UserServiceSession userServiceSession)
        {
            _supabaseClient = supabaseClient;
            _itemService = itemService;
            _userService = userServiceSession;  
        }

        // Form-bound properties

        [BindProperty]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Category is required.")]
        public int CategoryId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Make is required.")]
        public string Make { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Model is required.")]
        public string Model { get; set; }

        public List<Category> Categories { get; set; }
        Random rn = new Random();

        public async Task OnGetAsync()
        {
            // Fetch the categories from Supabase
            Categories = await _itemService.GetCategoriesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = await _itemService.GetCategoriesAsync();
                return Page();
            }

            // Capitalize Make and Model
            Make = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Make.ToLower());
            Model = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Model.ToLower());

            var newItem = new Item
            {
                Id = rn.Next(1, int.MaxValue),
                Name = this.Name,
                Description = this.Description,
                Category = this.CategoryId,
                Price = this.Price,
                Make = this.Make,
                Model = this.Model,
                CheckedIn = true, // Default value for CheckedIn
                CreatedAt = DateTime.UtcNow,
                //Creator = _userService.UserId, // Replace with the logged-in user's ID
            };

            await _itemService.AddItemAsync(newItem);

            return RedirectToPage("/Registered/LoggedInIndex");
        }
    }
}
