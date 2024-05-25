using Microsoft.EntityFrameworkCore;
using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Repositories
{
    public class RefundRepository : BaseRepository<Refund>
    {
        public RefundRepository(RefundManagementContext context) : base(context)
        {
        }
    }
}
