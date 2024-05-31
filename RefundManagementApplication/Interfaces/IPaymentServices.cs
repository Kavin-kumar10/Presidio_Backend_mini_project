using RefundManagementApplication.Models;

namespace RefundManagementApplication.Interfaces
{
    public interface IPaymentServices
    {
        public Task<Payment> GetMyPayment(int PaymentId);
        public Task<Payment> CreatePaymentToRefund(int AdminId, int refundId);
    }
}
