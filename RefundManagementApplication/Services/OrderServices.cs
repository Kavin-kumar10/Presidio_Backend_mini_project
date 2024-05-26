using RefundManagementApplication.Exceptions;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Services
{
    public class OrderServices : BaseServices<Order>, IOrderServices
    {
        IRepository<int, Order> _repo;    
        public OrderServices(IRepository<int, Order> repo) : base(repo)
        {
            _repo = repo;
        }

        public async Task<Order> UpdateOrderStatus(string Status, int OrderId)
        {
            var reqOrder = await _repo.Get(OrderId);
            if (reqOrder != null) {
                reqOrder.OrderStatus = Status;
                return await _repo.Update(reqOrder);
            }
            throw new NotFoundException("Order");
        }
    }
}
