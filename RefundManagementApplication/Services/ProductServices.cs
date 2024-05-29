using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Services
{
    public class ProductServices : BaseServices<Product>    
    {
        public ProductServices(IRepository<int, Product> repo) : base(repo)
        {
        }
    }
}
