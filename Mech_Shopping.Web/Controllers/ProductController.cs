using Mech_Shopping.Web.Models;
using Mech_Shopping.Web.Services.IServices;
using Mech_Shopping.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mech_Shopping.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }

        private async Task<string?> GetAccessTokenAsync()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            if (string.IsNullOrEmpty(token))
            {
                // token expirado → volta pro login
                return null;
            }

            return token;
        }

        // LISTAGEM
        [Authorize]
        public async Task<IActionResult> ProductIndex()
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return RedirectToAction("Login", "Account");

            var products = await _productService.FindAllProducts(token);
            return View(products);
        }

        // CRIAÇÃO - GET
        [Authorize]
        public IActionResult ProductCreate()
        {
            return View();
        }

        // CRIAÇÃO - POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProductCreate(ProductModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = await GetAccessTokenAsync();
            if (token == null) return RedirectToAction("Login", "Account");

            var response = await _productService.CreateProduct(model, token);
            if (response != null)
                return RedirectToAction(nameof(ProductIndex));

            return View(model);
        }

        // ATUALIZAÇÃO - GET
        [Authorize]
        public async Task<IActionResult> ProductUpdate(int id)
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return RedirectToAction("Login", "Account");

            var product = await _productService.FindProductById(id, token);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // ATUALIZAÇÃO - POST
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ProductUpdate(ProductModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var token = await GetAccessTokenAsync();
            if (token == null) return RedirectToAction("Login", "Account");

            var updated = await _productService.UpdateProduct(model, token);
            if (updated != null)
                return RedirectToAction(nameof(ProductIndex));

            return View(model);
        }

        // DELEÇÃO - GET (somente Admin)
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return RedirectToAction("Login", "Account");

            var product = await _productService.FindProductById(id, token);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // DELEÇÃO - POST (somente Admin)
        [HttpPost]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> ProductDeleteConfirmed(long id)
        {
            var token = await GetAccessTokenAsync();
            if (token == null) return RedirectToAction("Login", "Account");

            var result = await _productService.DeleteProductById(id, token);

            if (result)
                return RedirectToAction(nameof(ProductIndex));

            return BadRequest();
        }
    }
}
