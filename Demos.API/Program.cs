using Demos.Authorization;
using Demo.API.Authorization;
using Demo.API.Contracts;
using Demo.API.Helpers;
using Demo.API.Middlewares;
using Demo.API.Repository;
using Demo.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Events;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Identity.Web;

namespace Demo.API
{
    public class Program
    {
        const string CorsPolicyName = "CorsPolicy";

        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                //.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
                //.MinimumLevel.Override("Demo.API.Controllers.GamesController", LogEventLevel.Information)
                .WriteTo.Console(LogEventLevel.Debug)
                .WriteTo.File("logs/demosessions.txt", LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog();

            //builder.Logging.ClearProviders();
            //builder.Logging.AddConsole();

            // Add services to the container.
            builder.Services.ConfigureDb(builder.Configuration);

            //builder.Services.ConfigureCors(CorsPolicyName);

            builder.Services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;

            }).AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();            

            builder.Services.ConfigureSwagger();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IGameRepository, GameRepository>();
            //builder.Services.AddTransient<IPasswordHash, PasswordHasher>();

            builder.Services.AddSingleton<FileExtensionContentTypeProvider>();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new()
            //    {
            //        ValidateIssuer = true,
            //        ValidateAudience = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidIssuer = builder.Configuration["Authentication:Issuer"],
            //        ValidAudience = builder.Configuration["Authentication:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(
            //            Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
            //    };
            //});
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:5001";
                options.Audience = "demosapi";
                options.TokenValidationParameters = new()
                {
                    NameClaimType = "given_name",
                    RoleClaimType = "role",
                    ValidTypes = new[] { "at+jwt" }
                };
            });
            //.AddOAuth2Introspection(options =>
            //{
            //    options.Authority = "https://localhost:44300";
            //    options.ClientId = "demosapiclient";
            //    options.ClientSecret = "secret";
            //    options.NameClaimType = "given_name";
            //    options.RoleClaimType = "role";
            //});
            //.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

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

            builder.Services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
                setupAction.ApiVersionReader = ApiVersionReader.Combine(
                    new UrlSegmentApiVersionReader(), // /api/v1/*
                    new HeaderApiVersionReader("x-api-version"), // x-api-version:1.0
                    new MediaTypeApiVersionReader("x-api-version")); // Accept/Content-Type: application/json; x-api-version=1.0

            });

            builder.Services.AddVersionedApiExplorer(setup =>
            {
                setup.GroupNameFormat = "'v'VVV";
                setup.SubstituteApiVersionInUrl = true;
            });

            builder.Services.ConfigureOptions<SwaggerOptions>();

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

            var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

            app.UseSwaggerUI(options =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = "";
            });
            //}

            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable cors to all, after routing but before authorization
            // app.UseCors();


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