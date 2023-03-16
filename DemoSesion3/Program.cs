using DemoSesion3.Contracts;
using DemoSesion3.Helpers;
using DemoSesion3.Repository;
using DemoSesion3.Services;
using Microsoft.AspNetCore.StaticFiles;
using Serilog;

namespace DemoSesion3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/demosessions.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);


            builder.Host.UseSerilog();

            //builder.Logging.ClearProviders();
            //builder.Logging.AddConsole();

            // Add services to the container.
            builder.Services.ConfigureDb(builder.Configuration);            

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
            }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();            

            builder.Services.ConfigureSwagger();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

                app.UseSwagger(options =>
                {
                    options.SerializeAsV2 = true;
                });

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = "api/doc";
                });
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