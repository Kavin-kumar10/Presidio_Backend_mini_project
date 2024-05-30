using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.OrderReqDTOs;
using RefundManagementApplication.Models.Enums;

namespace RefundManagementApplication.Interfaces
{
    public interface IOrderServices
    {
        public Task<Order> CreateOrder(OrderRequestDTO orderRequestDTO);
        public Task<Order> UpdateOrderStatus(OrderStatuses Status, int OrderId);
        public Task<IEnumerable<Order>> GetAllRefundDecisionPendingOrders();
        public Task<IEnumerable<Order>> GetAllRefundDecisionAcceptedOrders();
    }
}
