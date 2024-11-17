using DatabaseApplication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static DatabaseApplication.NewFolder.NewUser;

namespace DatabaseApplication.Pages
{
    public class SignupModel : PageModel
    {

        private readonly ApplicationDbContext _context;

        public SignupModel(ApplicationDbContext context)
        {
            _context = context;
        }


        [BindProperty]
        [Column("first_name")]
        [Required]
        public string first_name { get; set; }

        [BindProperty]
        [Column("last_name")]
        [Required]
        public string last_name { get; set; }

        [BindProperty]
        [Required]
        [Column("email")]
        [EmailAddress]
        public string email { get; set; }

        [BindProperty]
        [Required]
        public string password { get; set; }

        public string hashed_password { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new User
            {
                first_name = first_name,
                last_name = last_name,
                email = email,
                password = password, // Save the hashed password
                hashed_password = hashedPassword
            };

            // Add the user to the database
            _context.Users.Add(user);
            _context.SaveChanges();

            // Handle the registration logic here (e.g., saving to a database).
            // For demonstration, we'll just show a success message.
            Message = "Registration successful!";

            // Optionally, redirect to another page or show a confirmation.
            return RedirectToPage("/Index");

        }
    }
}
