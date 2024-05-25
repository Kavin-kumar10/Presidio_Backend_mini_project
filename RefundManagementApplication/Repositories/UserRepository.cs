using RefundManagementApplication.Context;
using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(RefundManagementContext context) : base(context)
        {
        }
    }
}
