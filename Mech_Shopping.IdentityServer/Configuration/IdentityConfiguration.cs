using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mech_Shopping.IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        public const string Admin = "Admin";
        public const string Client = "Client";

        // Identity resources (OpenID Connect standard)
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
            // Machine-to-machine client
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("my_super_secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "read", "write" } // apenas escopos necessários
            },

            // OIDC client with PKCE + refresh tokens
            new Client
            {
                ClientId = "mech_shopping",
                ClientSecrets = { new Secret("my_super_secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code, // Authorization Code
                RequirePkce = true,                  // PKCE obrigatório
                RequireClientSecret = true,          // para segurança
                AllowOfflineAccess = true,           // habilita refresh token

                RedirectUris = { "https://localhost:4430/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:4430/signout-callback-oidc" },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "mech_shopping"
                },

                // Opcional: limita o tempo de vida do token de refresh
                RefreshTokenExpiration = TokenExpiration.Absolute,
                AbsoluteRefreshTokenLifetime = 2592000, // 30 dias em segundos
                RefreshTokenUsage = TokenUsage.OneTimeOnly, // refresh token único por uso
            }
        };
    }
}
