using RefundManagementApplication.Context;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Repositories
{
    public class OrderRepository : BaseRepository<Order>
    {
        public OrderRepository(RefundManagementContext context) : base(context)
        {
        }
    }
}
