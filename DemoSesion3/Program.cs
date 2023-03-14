
using DemoSesion3.Context;
using DemoSesion3.Helpers;
using DemoSesion3.Migrations;
using DemoSesion3.Services;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace DemoSesion3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSingleton<DapperContext>();
            builder.Services.AddSingleton<Database>();
            builder.Services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddSqlServer2016()
                    .WithGlobalConnectionString(builder.Configuration.GetConnectionString("SqlConnection"))
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();            

            builder.Services.ConfigureSwagger();

            builder.Services.AddSingleton<UsersDataStore>();
            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

#if DEBUG
            builder.Services.AddTransient<INotificationService, LocalNotificationService>();
#else
            builder.Services.AddTransient<INotificationService, CloudNotificationService>();
#endif

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.MigrateDatabase();

            app.Run();
        }
    }
}