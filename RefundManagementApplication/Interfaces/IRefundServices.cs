using RefundManagementApplication.Models;

namespace RefundManagementApplication.Interfaces
{
    public interface IRefundServices
    {
        public Task<Refund> CreateRefund(int OrderId, string Reason);
        public Task<IEnumerable<Refund>> GetAllRefundsById(int MemberId);
    }
}
