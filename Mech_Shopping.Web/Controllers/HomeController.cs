using System.Diagnostics;
using Mech_Shopping.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mech_Shopping.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        // Mantido conforme pedido — apenas corrigido e tornado útil
        [Authorize]
        public async Task<IActionResult> Login()
        {
            // Recupera o access token
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            // Aqui você pode logar, debugar ou validar o token se quiser
            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("Token não encontrado ou expirado no método Login().");
            }

            // Após autenticação, volta para a página inicial
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            return SignOut(
                new AuthenticationProperties { RedirectUri = "/" },
                "Cookies",
                "oidc"
            );
        }
    }
}
