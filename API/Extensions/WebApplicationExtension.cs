using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class WebApplicationExtension
    {
        public static WebApplication MigrateDatabase<T>(this WebApplication webHost) where T : DbContext
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILoggerManager>();
                    logger.LogError(ex, $"An error occurred while migrating the database.");
                }
            }
            return webHost;
        }
    }
}
