namespace DatabaseApplication.Services
{
    public class MiddlewareService
    {
        private readonly RequestDelegate _next;
        private readonly UserServiceSession _userSessionService;

        public MiddlewareService(RequestDelegate next, UserServiceSession userSessionService)
        {
            _next = next;
            _userSessionService = userSessionService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if the request is for the Index page
            if (context.Request.Path == "/Index" && _userSessionService.IsLoggedIn)
            {
                context.Response.Redirect("/Registered/LoggedInIndex");
                return;
            }

            await _next(context);
        }



    }
}
