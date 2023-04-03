// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using IdentityModel;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer;
using Duende.IdentityServer.Test;

namespace Demos.IDP;

public class TestUsers
{
    public static List<TestUser> Users
    {
        get
        {
            var address = new
            {
                street_address = "One Hacker Way",
                locality = "Heidelberg",
                postal_code = 69118,
                country = "Germany"
            };
                
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "67fbac34-1ee1-4697-b916-1748861dd275",
                    Username = "Gandalf",
                    Password = "thering",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Gandalf The White"),
                        new Claim(JwtClaimTypes.GivenName, "Gandalf"),
                        new Claim(JwtClaimTypes.FamilyName, "The White"),
                        new Claim(JwtClaimTypes.Email, "gandalf@mage.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    }
                },
                new TestUser
                {
                    SubjectId = "70dc14ae-78cb-41fc-a675-02a7991660ff",
                    Username = "Frodo",
                    Password = "theshire",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Frodo Baggins"),
                        new Claim(JwtClaimTypes.GivenName, "Frodo"),
                        new Claim(JwtClaimTypes.FamilyName, "Baggins"),
                        new Claim(JwtClaimTypes.Email, "frodo@theshire.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    }
                },
                new TestUser
                {
                    SubjectId = "ba434a19-4e5d-49d6-98e0-43ff2b76482d",
                    Username = "Trancos",
                    Password = "middleearth",
                    Claims =
                    {
                        new Claim(JwtClaimTypes.Name, "Aragorn II"),
                        new Claim(JwtClaimTypes.GivenName, "Aragorn "),
                        new Claim(JwtClaimTypes.FamilyName, "II"),
                        new Claim(JwtClaimTypes.Email, "king@middleearth.com"),
                        new Claim(JwtClaimTypes.EmailVerified, "true", ClaimValueTypes.Boolean),
                    }
                }
            };
        }
    }
}