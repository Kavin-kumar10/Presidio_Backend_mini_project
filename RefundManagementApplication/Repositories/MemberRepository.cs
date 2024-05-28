using RefundManagementApplication.Context;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Repositories
{
    public class MemberRepository:BaseRepository<Member>
    {

        public MemberRepository(RefundManagementContext context) : base(context)
        {
        }
    }
}
