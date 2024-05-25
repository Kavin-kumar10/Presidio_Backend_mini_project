using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Exceptions.ProductExceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Services
{
    public class ProductServices : IProductServices
    {
        private readonly IRepository<int, Product> _repo;

        public ProductServices(IRepository<int,Product> repo) {
            _repo = repo;
        }

        public async Task<IEnumerable<Product>> GetAllProducts() {
            var result = await _repo.Get();
            if(result != null)
            {
                return result;
            }
            throw new ProductNotFoundException();
        }

        public async Task<Product> CreateProduct(Product product)
        {
            var result = await _repo.Add(product);
            if(result != null)
            {
                return result;
            }
            throw new NotImplementedException();
        }

        public async Task<Product> GetProductById(int id)
        {
            var result = await _repo.Get(id);
            if(result != null)
            {
                return result;
            }
            throw new ProductNotFoundException();
        }

        public Task<Product> UpdateProduct(Product product)
        {
            var result = _repo.Update(product);
            if(result != null)
            {
                return result;
            }
            throw new ProductNotFoundException();
        }

        public Task<Product> DeleteProduct(Product product)
        {
            var result = _repo.Delete(product.ProductId);
            if(result != null)
                return result;
            throw new ProductNotFoundException();
        }
    }
}
