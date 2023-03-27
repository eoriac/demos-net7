using Demo.API.Context;
using Demo.API.Migrations;
using FluentMigrator.Runner;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Demo.API.Helpers
{
    public static class ServiceExtensions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                options.IncludeXmlComments(xmlCommentsFullPath);

                options.AddSecurityDefinition("GamesApiBearerAuth", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.Http,                    
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "A valid token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "GamesApiBearerAuth"
                            }
                        }, new List<string>()
                    }
                });                
            });
        }

        public static void ConfigureCors(this IServiceCollection services, string policyName)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(policyName,
                    builder => builder.AllowAnyOrigin()
                        //.WithOrigins("https://localhost:75236",
                        //             "https://blog.azure.com")
                        .WithMethods("PUT", "GET")
                        .AllowAnyHeader()
                        .AllowCredentials());

                options.AddDefaultPolicy(
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:75236", "https://blog.azure.com");
                    });
            });
        }

        public static void ConfigureDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DapperContext>();
            services.AddSingleton<Database>();

            services.AddLogging(c => c.AddFluentMigratorConsole())
                .AddFluentMigratorCore()
                .ConfigureRunner(c => c.AddSqlServer2016()
                    .WithGlobalConnectionString(configuration.GetConnectionString("SqlConnection"))
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations());
        }
    }
}
