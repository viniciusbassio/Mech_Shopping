using Mech_Shopping.Web.Services;
using Mech_Shopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Text.Json;

namespace Mech_Shopping.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // MVC
            builder.Services.AddControllersWithViews();

            //
            // =============================
            //   AUTENTICAÇÃO OIDC
            // =============================
            //
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies", c =>
            {
                c.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                c.SlidingExpiration = true;
            })
            .AddOpenIdConnect("oidc", options =>
            {
                //
                // Authority DEFINITIVO — carregado do appsettings
                //
                options.Authority = builder.Configuration["ServiceUrls:IdentityServer"]!;
                options.RequireHttpsMetadata = false;

                //
                // Aceitar certificado local (SOMENTE DEV)
                //
                options.BackchannelHttpHandler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                //
                // Eventos de diagnóstico
                //
                options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
                {
                    OnAuthorizationCodeReceived = async ctx =>
                    {
                        try
                        {
                            var config = await ctx.Options.ConfigurationManager
                                .GetConfigurationAsync(default);

                            Console.WriteLine("[OIDC DIAG] Authority: " + ctx.Options.Authority);
                            Console.WriteLine("[OIDC DIAG] token_endpoint: " + config.TokenEndpoint);

                            Console.WriteLine("[OIDC DIAG] ProtocolMessage: " +
                                JsonSerializer.Serialize(new
                                {
                                    ctx.ProtocolMessage.Code,
                                    ctx.ProtocolMessage.State,
                                    ctx.ProtocolMessage.Error,
                                    ctx.ProtocolMessage.ErrorDescription
                                }));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("[OIDC DIAG] Config error: " + ex);
                        }
                    },

                    OnRemoteFailure = ctx =>
                    {
                        Console.WriteLine("[OIDC DIAG] Remote failure: " + ctx.Failure?.Message);
                        Console.WriteLine("[OIDC DIAG] Exception details: " + ctx.Failure);

                        if (ctx.Properties != null)
                        {
                            Console.WriteLine("[OIDC DIAG] Properties:");
                            foreach (var prop in ctx.Properties.Items)
                                Console.WriteLine($"   {prop.Key} = {prop.Value}");
                        }

                        ctx.HandleResponse();
                        return Task.CompletedTask;
                    }
                };

                //
                // Configurações do cliente
                //
                options.ClientId = "mech_shopping";
                options.ClientSecret = "my_super_secret";
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.UsePkce = true;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("mech_shopping");
                options.Scope.Add("offline_access");

                options.GetClaimsFromUserInfoEndpoint = true;

                options.ClaimActions.MapJsonKey("role", "role", "role");
                options.ClaimActions.MapJsonKey("sub", "sub", "sub");

                options.TokenValidationParameters.NameClaimType = "name";
                options.TokenValidationParameters.RoleClaimType = "role";

                options.SaveTokens = true;

                options.CallbackPath = "/signin-oidc";
                options.SignedOutCallbackPath = "/signout-callback-oidc";
            });

            //
            // HttpClient de API
            //
            builder.Services.AddHttpClient<IProductService, ProductService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:ProductAPI"]!);
            });

            var app = builder.Build();

            //
            // PIPELINE
            //
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
