using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Repositories
{
    public class OrderRepository : BaseRepository<Order>
    {
        RefundManagementContext _context;
        public OrderRepository(RefundManagementContext context) : base(context)
        {
            _context = context; 
        }
        public override async Task<IEnumerable<Order>> Get()
        {
            var result = await _context.Orders.Include(o=>o.product).Include(o=>o.OrderedBy).ToListAsync();
            return result;
        }
    }
}
