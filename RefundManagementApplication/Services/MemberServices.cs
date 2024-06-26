using RefundManagementApplication.Interfaces;
using RefundManagementApplication.Models;

namespace RefundManagementApplication.Services
{
    public class MemberServices : BaseServices<Member>
    {
        public MemberServices(IRepository<int, Member> repo) : base(repo)
        {
        }
    }
}
