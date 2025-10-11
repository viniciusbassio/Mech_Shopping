using Mech_Shopping.Web.Utils;

namespace Mech_Shopping.Web.Models.IServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient client;
        
        public const string BasePath = "api/v1/products";
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
            var response = await client.PostAsJson(BasePath, productModel);
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
            var response = await client.PutAsJson($"{BasePath}/{productModel.Id}", productModel);
            if (response.IsSuccessStatusCode)
            {
                return await response.ReadContentAs<ProductModel>();
            }
            else
            {
                throw new Exception("Something went wrong when calling API");
            }
        }

        public async Task<bool> DeleteProductById(long id)
        {
            var response = await client.DeleteAsync($"{BasePath}/{id}");
            return response.IsSuccessStatusCode;
        }


        
    }
}
