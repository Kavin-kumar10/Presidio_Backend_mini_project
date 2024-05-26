using RefundManagementApplication.Models;

namespace RefundManagementApplication.Interfaces
{
    public interface IOrderServices
    {
        public Task<Order> UpdateOrderStatus(string Status, int OrderId);

    }
}
