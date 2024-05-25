using RefundManagementApplication.Models;

namespace RefundManagementApplication.Interfaces
{
    public interface ITokenServices
    {
        public string GenerateToken(Member member);
    }
}
