using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer;

public class Config
{
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "shoppingClient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256(), expiration: DateTime.UtcNow.AddHours(1)) },
                AllowedScopes = { "shoppingApi" }
            },
            new Client
            {
                ClientId = "shopping_mvc_client",
                ClientName = "Shopping MVC Web App",
                AllowedGrantTypes = GrantTypes.Hybrid,
                RequirePkce = false,
                AllowRememberConsent = false,
                RedirectUris = new List<string> { "https://localhost:5010/signin-oidc" }, // TODO: verify this url
                PostLogoutRedirectUris = new List<string> { "https://localhost:5010/signout-callback-oidc" }, // TODO: verify this url
                ClientSecrets = new List<Secret> { new Secret("secret".Sha256(), expiration: DateTime.UtcNow.AddHours(1)) },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Address,
                    IdentityServerConstants.StandardScopes.Email,
                    "shoppingApi",
                    "roles"
                }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
       new ApiScope[]
       {
            new ApiScope("shoppingApi", "Shopping API")
       };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource("shoppingApi", "Shopping API")
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResources.Address(),
            new IdentityResources.Email(),
            new IdentityResource(
                "roles",
                "Your role(s)",
                new List<string>() { "role" })
        };

    public static List<TestUser> TestUsers =>
        new()
        {
            new TestUser
            {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "jb",
                Password = "password",
                Claims = new List<Claim>
                {
                    new Claim(JwtClaimTypes.GivenName, "john"),
                    new Claim(JwtClaimTypes.FamilyName, "b")
                }
            }
        };
}
