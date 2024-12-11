using DatabaseApplication.NewFolder;
using DatabaseApplication.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using static Supabase.Postgrest.Constants;

namespace DatabaseApplication.Pages.Shared
{
    public class _LayoutLoggedInModel : PageModel
    {
        private readonly Supabase.Client _supabaseClient;
        private readonly UserServiceSession _userServiceSession;

        public int? AuthorizationLevel { get; private set; }

        public _LayoutLoggedInModel(Supabase.Client supabaseClient, UserServiceSession userServiceSession)
        {
            _supabaseClient = supabaseClient;
            _userServiceSession = userServiceSession;
        }

        public async Task OnGetAsync()
        {
            await InitializeAuthorizationAsync();
        }

        private async Task InitializeAuthorizationAsync()
        {
            // Fetch the logged-in user's details
            var userId = _userServiceSession.UserId; // Assuming UserId is an int
            if (userId <= 0) // Ensure UserId is valid
            {
                AuthorizationLevel = 0; // Default to no access if userId is invalid
                return;
            }

            var userResponse = await _supabaseClient
                .From<NewUser.User>()
                .Filter("id", Operator.Equals, userId)
                .Get();

            var user = userResponse.Models.FirstOrDefault();
            AuthorizationLevel = user?.Autherization ?? 0; // Default to 0 if no authorization value is found
        }
    }
}
