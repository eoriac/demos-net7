using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace Demos.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services
                .AddControllersWithViews()
                .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNameCaseInsensitive = true);

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            builder.Services.AddAccessTokenManagement();

            builder.Services.AddHttpClient("DemosAPIClient", client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["DemosAPIUrl"]);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            }).AddUserAccessTokenHandler();

            //builder.Services.AddHttpClient("IDPClient", client =>
            //{
            //    client.BaseAddress = new Uri("https://localhost:44300/");
            //});

            //builder.Services.AddHttpClient("OpenWeatherAPIClient", client =>
            //{
            //    client.BaseAddress = new Uri(builder.Configuration["OPWAPIUrl"]);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/json");
            //});

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                {
                    options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.Authority = "https://localhost:5001/";
                    options.ClientId = "demosapiclient"; // same that the one on IDP
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";

                    // not needed
                    // options.Scope.Add("openid");
                    // options.Scope.Add("profile");

                    //options.CallbackPath = new PathString("signin-oidc");

                    // save the token received from IDP
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClaimActions.Remove("aud");
                    options.ClaimActions.DeleteClaim("sid");
                    options.ClaimActions.DeleteClaim("idp");
                    options.Scope.Add("roles");
                    options.Scope.Add("demosapi.fullaccess");
                    options.ClaimActions.MapJsonKey("role", "role");
                    options.TokenValidationParameters = new()
                    {
                        NameClaimType = "given_name",
                        RoleClaimType = "role",
                    };
                });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}