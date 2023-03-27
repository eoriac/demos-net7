using Duende.IdentityServer.Models;

namespace Demos.IDP;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId()
        };

    public static IEnumerable<ApiResource> ApiResources =>
         new ApiResource[]
             {
                     new ApiResource("gamesapi",
                         "Games API",
                         new [] { "role", "age" })
                     {
                         Scopes = { "gamesapi.fullaccess",
                             "gamesapi.read",
                             "gamesapi.write"},
                        ApiSecrets = { new Secret("apisecret".Sha256()) }
                     }
             };


    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            {
                new ApiScope("gamesapi.fullaccess"),
                new ApiScope("gamesapi.read"),
                new ApiScope("gamesapi.write")};

    public static IEnumerable<Client> Clients =>
        new Client[] 
            { 

            };
}