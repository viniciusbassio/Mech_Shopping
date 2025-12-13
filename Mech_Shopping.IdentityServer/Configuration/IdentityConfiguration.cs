using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mech_Shopping.IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Client = "Client";

        // Identity resources
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email()
            };

        // API scopes
        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("mech_shopping", "Mech Shopping API"),
            new ApiScope("read", "Read your data."),
            new ApiScope("write", "Write your data."),
            new ApiScope("delete", "Delete your data.")
        };

        // Clients
        public static IEnumerable<Client> Clients => new List<Client>
        {
            // Machine to Machine
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("my_super_secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "read", "write" }
            },

            // OIDC Client
            new Client
            {
                ClientId = "mech_shopping",
                ClientSecrets = { new Secret("my_super_secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = true,

                AllowOfflineAccess = true, // habilita refresh token

                RedirectUris =
                {
                    "https://localhost:4430/signin-oidc"
                },
                PostLogoutRedirectUris =
                {
                    "https://localhost:4430/signout-callback-oidc"
                },

                // ❗ Se estiver usando MVC, isso é importante:
                FrontChannelLogoutUri = "https://localhost:4430/signout-oidc",

                // Permitir tokens via navegador (caso use SPA ou MVC)
                AllowAccessTokensViaBrowser = true,

                // IMPORTANTE: senão o IdentityServer bloqueia a volta para o client
                AllowedCorsOrigins =
                {
                    "https://localhost:4430"
                },

                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "mech_shopping"
                },

                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 2592000, // 30 dias
                RefreshTokenUsage = TokenUsage.OneTimeOnly
            },

            // Postman
            new Client
{
    ClientId = "postman_client",
    ClientSecrets = { new Secret("postman_secret".Sha256()) },
    AllowedGrantTypes = GrantTypes.ClientCredentials,
    AllowedScopes = { "read", "write", "mech_shopping" }
}
        };
    }
}
