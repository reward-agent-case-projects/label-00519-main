using Hangfire.Dashboard;

namespace Backend.Api.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // In a real application, you would check for authentication here.
            // For this example, we allow all local requests or implement Basic Auth.
            // CAUTION: This allows public access if not behind a firewall or auth middleware.
            return true; 
        }
    }
}
