using RefundManagementApplication.Context;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>
    {
        public PaymentRepository(RefundManagementContext context) : base(context)
        {
        }
    }
}
