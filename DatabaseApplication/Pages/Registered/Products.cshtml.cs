using DatabaseApplication.Data;
using DatabaseApplication.EntityModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DatabaseApplication.Pages.Registered
{
    public class ProductsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ProductsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // List of Items to display on the page
        public List<Item> Items { get; set; }

        public void OnGet()
        {
            // Fetch all items from the database
            Items = _context.Items.ToList();
        }
    }
}
