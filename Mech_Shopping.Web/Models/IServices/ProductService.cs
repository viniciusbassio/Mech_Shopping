using Mech_Shopping.Web.Utils;

namespace Mech_Shopping.Web.Models.IServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient client;
        
        public const string BasePath = "api/v1/Product";
        public ProductService(HttpClient httpClient)
        {
            client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<IEnumerable<ProductModel>> FindAllProducts()
        {
            var response = await client.GetAsync(BasePath);
            return await response.ReadContentAs<List<ProductModel>>();
        }

        public async Task<ProductModel> FindProductById(long id)
        {
            var response = await client.GetAsync($"{BasePath}/{id}");
            return await response.ReadContentAs<ProductModel>();
        }
        public async Task<ProductModel> CreateProduct(ProductModel productModel)
        {
            var response = await client.PostAsJsonAsync($"{BasePath}", productModel);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Erro da API ao criar produto:");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine($"Resposta: {content}");
            }

            if (response.IsSuccessStatusCode)
            {
                return await response.ReadContentAs<ProductModel>();
            }
            else
            {
                throw new Exception("Something went wrong when calling API");
            }
        }

        public async Task<ProductModel> UpdateProduct(ProductModel productModel)
        {
            var response = await client.PutAsJsonAsync($"{BasePath}/{productModel.Id}", productModel);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Erro da API ao atualizar produto:");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine($"Resposta: {content}");

                throw new ApplicationException($"Erro ao atualizar produto: {response.ReasonPhrase}");
            }

            return await response.ReadContentAs<ProductModel>();
        }

        public async Task<bool> DeleteProductById(long id)
        {
            var response = await client.DeleteAsync($"{BasePath}/{id}");
            if (response.IsSuccessStatusCode)
                {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"❌ Erro da API ao deletar produto:");
                Console.WriteLine($"StatusCode: {response.StatusCode}");
                Console.WriteLine($"Resposta: {content}");
            }
            else
                throw new ApplicationException($"Erro ao deletar produto: {response.ReasonPhrase}");
            return response.IsSuccessStatusCode;
        }


        
    }
}
