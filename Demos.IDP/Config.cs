using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Demos.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource("roles",
                "Config Roles",
                new [] { "role" }),
        };

    public static IEnumerable<ApiResource> ApiResources =>
         new ApiResource[]
             {
                new ApiResource("demosapi", "Demos API")
                {
                    Scopes = { 
                        "demosapi.fullaccess",
                        "demosapi.read",
                        "demosapi.write"
                    },
                    ApiSecrets = { new Secret("apisecret".Sha256()) }
                }
             };


    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("demosapi.fullaccess"),
                //new ApiScope("demosapi.read"),
                //new ApiScope("demosapi.write")
            };

    public static IEnumerable<Client> Clients =>
        new Client[] 
            { 
                new Client()
                {
                    ClientName = "Demos Client",
                    ClientId = "demosapiclient",
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris =
                    {
                        "https://localhost:7073/signin-oidc"
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "demosapi.fullaccess"
                    },
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    RequireConsent = true,
                }
            };
}