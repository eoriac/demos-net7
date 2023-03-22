using Demos.Authorization;
using DemoSesion3.Authorization;
using DemoSesion3.Contracts;
using DemoSesion3.Helpers;
using DemoSesion3.Middlewares;
using DemoSesion3.Repository;
using DemoSesion3.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;

namespace DemoSesion3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                //.MinimumLevel.Override("DemoSesion3.Controllers.GamesController", LogEventLevel.Information)
                .WriteTo.Console(LogEventLevel.Debug)
                .WriteTo.File("logs/demosessions.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
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
            builder.Services.AddTransient<IPasswordHash, PasswordHasher>();

            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration["Authentication:Issuer"],
                        ValidAudience = builder.Configuration["Authentication:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
                    };
                });

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IAuthorizationHandler, MustOwnGameHandler>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "UserCanAddGame", AuthorizationPolicies.CanAddGame());

                options.AddPolicy("MustOwnGame", policyBuilder =>
                {
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.AddRequirements(new MustOwnGameRequirement());
                });
            });

#if DEBUG
            builder.Services.AddTransient<INotificationService, LocalNotificationService>();
#else
            builder.Services.AddTransient<INotificationService, CloudNotificationService>();
#endif

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            // Commented because of "Local" environment

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
                //app.UseSwaggerUI();

                //app.UseSwagger(options =>
                //{
                //    options.SerializeAsV2 = true;
                //});

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                options.RoutePrefix = "";
            });
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.Use((ctx, next) =>
            //{
            //    var headers = ctx.Response.Headers;

            //    headers.Add("X-Frame-Options", "DENY");
            //    headers.Add("X-XSS-Protection", "1; mode=block");
            //    headers.Add("X-Content-Type-Options", "nosniff");
            //    headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains; preload");

            //    headers.Remove("X-Powered-By");
            //    headers.Remove("x-aspnet-version");

            //    // Some headers won't remove
            //    headers.Remove("Server");

            //    return next();
            //});
            app.UseMiddleware<CustomMiddleware>();

            app.UseAuthentication();

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