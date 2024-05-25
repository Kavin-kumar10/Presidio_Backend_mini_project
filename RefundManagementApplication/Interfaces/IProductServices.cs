using RefundManagementApplication.Models;

namespace RefundManagementApplication.Interfaces
{
    public interface IProductServices
    {
        public Task<IEnumerable<Product>> GetAllProducts();
        public Task<Product> GetProductById(int id);
        public Task<Product> CreateProduct(Product product);
        public Task<Product> UpdateProduct(Product product);
        public Task<Product> DeleteProduct(Product product);
    }
}
