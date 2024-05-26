using RefundManagementApplication.Models;

namespace RefundManagementApplication.Interfaces
{
    public interface IProductServices
    {
        public Task<IEnumerable<Product>> GetAll();
        public Task<Product> GetById(int id);
        public Task<Product> Create(Product product);
        public Task<Product> Update(Product product);
        public Task<Product> Delete(int Key);
    }
}
