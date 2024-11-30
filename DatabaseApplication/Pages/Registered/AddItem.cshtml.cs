using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace DatabaseApplication.Pages.Registered
{
    public class AddItemModel : PageModel
    {

        private readonly ApplicationDbContext _context;
        private readonly UserServiceSession _userServiceSession;


        public AddItemModel(ApplicationDbContext context, UserServiceSession userServiceSession)
        {
            _context = context;
            _userServiceSession = userServiceSession;
        }

        // Form-bound properties

        [BindProperty]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Category is required.")]
        public string Category { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Stock is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be greater than 0.")]
        public long Stock { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public double Price { get; set; }

        [BindProperty]
        public string? Photo { get; set; } // Nullable property since Photo is optional

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var newItem = new Item
            {
                Name = this.Name,
                Description = this.Description,
                Category = this.Category,
                Stock = this.Stock,
                Price = this.Price,
                Photo = this.Photo, // Optional field
                CreatedAt = DateTime.UtcNow,
                Creator = 7 // Replace with the logged-in user's ID
            };

            _context.Items.Add(newItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Registered/LoggedInIndex");
        }
    }
}
