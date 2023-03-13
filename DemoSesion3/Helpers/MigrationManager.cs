using DemoSesion3.Migrations;

namespace DemoSesion3.Helpers
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<Database>();

                try
                {
                    databaseService.CreateDatabase("DapperDemoSessions");
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return webApp;
        }
    }
}
