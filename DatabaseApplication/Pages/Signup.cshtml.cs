using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using static DatabaseApplication.NewFolder.NewUser;

namespace DatabaseApplication.Pages
{
    public class SignupModel : PageModel
    {
        private readonly Supabase.Client _supabaseClient;

        public SignupModel(Supabase.Client supabaseClient)
        {
            _supabaseClient = supabaseClient;
        }

        [BindProperty]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [BindProperty]
        [Required]
        [EmailAddress]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        public string Message { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Hash the password before saving
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);

            var random = new Random();

            var user = new User
            {
                Id = random.Next(1, int.MaxValue),
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password, // Save plain password (not recommended for production)
                HashedPassword = hashedPassword,
                Autherization = 0// Save hashed password
            };

            try
            {
                // Save the user to the Supabase "Users" table
                var response = await _supabaseClient.From<User>().Insert(user);

                if (response.Models.Count > 0)
                {
                    Message = "Registration successful!";
                    return RedirectToPage("/Index");
                }
                else
                {
                    Message = "Registration failed. Please try again.";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                // Log error and return failure message
                Console.WriteLine($"Error during registration: {ex.Message}");
                Message = "An error occurred. Please try again.";
                return Page();
            }
        }
    }
}