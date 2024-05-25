using RefundManagementApplication.Context;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Repositories
{
    public class ProductRepository : BaseRepository<Product>
    {
        public ProductRepository(RefundManagementContext context) : base(context)
        {
        }
    }
}
