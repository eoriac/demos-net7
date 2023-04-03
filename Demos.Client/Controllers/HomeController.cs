using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text.Json;
using System.Text;
using Demos.Client.ViewModels;
using Demos.Client.Models;

namespace Demos.Client.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly ILogger<HomeController> logger;

        public HomeController(IHttpClientFactory httpClientFactory, ILogger<HomeController> logger)
        {
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index()
        {
            await LogIdentityInformation();

            var httpClient = httpClientFactory.CreateClient("DemosAPIClient");

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/users/");
            
            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            response.EnsureSuccessStatusCode();

            var responseContent= await response.Content.ReadAsStringAsync();
            
            var users = JsonSerializer.Deserialize<List<UserModel>>(responseContent);
            return View(new UsersViewModel(users ?? new List<UserModel>()));
            
        }

        public async Task LogIdentityInformation()
        {
            var identityToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            var accessToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);

            var refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var userClaimsStringBuilder = new StringBuilder();

            foreach (var claim in User.Claims)
            {
                userClaimsStringBuilder.AppendLine($"Claim type: {claim.Type} \n Claim value: {claim.Value}");
            }

            // log token & claims
            logger.LogInformation($"ID token + Claims: {identityToken} \n{userClaimsStringBuilder}");
            logger.LogInformation($"Access token: {accessToken}");
            logger.LogInformation($"Refresh token: {refreshToken}");
        }
    }
}