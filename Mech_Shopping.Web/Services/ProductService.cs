using Mech_Shopping.Web.Models;
using Mech_Shopping.Web.Services.IServices;
using Mech_Shopping.Web.Utils;
using System.Net.Http.Headers;

namespace Mech_Shopping.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _client;

        private const string BasePath = "api/v1/Product";

        public ProductService(HttpClient httpClient)
        {
            _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private void AddAuthorizationHeader(string token)
        {
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<IEnumerable<ProductModel>> FindAllProducts(string token)
        {
            AddAuthorizationHeader(token);

            var response = await _client.GetAsync(BasePath);
            return await response.ReadContentAs<List<ProductModel>>();
        }

        public async Task<ProductModel> FindProductById(long id, string token)
        {
            AddAuthorizationHeader(token);

            var response = await _client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContentAs<ProductModel>();
        }

        public async Task<ProductModel> CreateProduct(ProductModel model, string token)
        {
            AddAuthorizationHeader(token);

            var response = await _client.PostAsJsonAsync(BasePath, model);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("❌ Erro da API ao criar produto:");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine($"Resposta: {content}");

                throw new ApplicationException($"Erro ao criar produto: {response.ReasonPhrase}");
            }

            return await response.ReadContentAs<ProductModel>();
        }

        public async Task<ProductModel> UpdateProduct(ProductModel model, string token)
        {
            AddAuthorizationHeader(token);

            var response = await _client.PutAsJsonAsync($"{BasePath}/{model.Id}", model);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("❌ Erro da API ao atualizar produto:");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine($"Resposta: {content}");

                throw new ApplicationException($"Erro ao atualizar produto: {response.ReasonPhrase}");
            }

            return await response.ReadContentAs<ProductModel>();
        }

        public async Task<bool> DeleteProductById(long id, string token)
        {
            AddAuthorizationHeader(token);

            var response = await _client.DeleteAsync($"{BasePath}/{id}");

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine("❌ Erro da API ao deletar produto:");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine($"Resposta: {content}");

                throw new ApplicationException($"Erro ao deletar produto: {response.ReasonPhrase}");
            }

            return true;
        }
    }
}
