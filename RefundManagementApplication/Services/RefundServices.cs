using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Services
{
    public class RefundServices : BaseServices<Refund>
    {
        public RefundServices(IRepository<int, Refund> repo) : base(repo)
        {
        }
    }
}
