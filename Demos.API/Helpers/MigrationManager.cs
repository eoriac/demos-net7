using Demo.API.Migrations;
using FluentMigrator.Runner;

namespace Demo.API.Helpers
{
    public static class MigrationManager
    {
        public static WebApplication MigrateDatabase(this WebApplication webApp)
        {
            using (var scope = webApp.Services.CreateScope())
            {
                var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    databaseService.CreateDatabase("DemoSessions");
                    migrationService.ListMigrations();
                    // Reset tables apply migration InitialTables down
                    // migrationService.MigrateDown(2023130300000);
                    migrationService.MigrateUp();
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
