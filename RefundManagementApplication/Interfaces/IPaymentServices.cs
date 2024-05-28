using RefundManagementApplication.Models;

namespace RefundManagementApplication.Interfaces
{
    public interface IPaymentServices
    {
        public Task<Payment> CreatePaymentToRefund(int AdminId, int refundId);
    }
}
