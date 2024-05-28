using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.ResponseDTO.Activation;
using RefundManagementApplication.Models.Enums;

namespace RefundManagementApplication.Interfaces
{
    public interface IActivateServices
    {
        public Task<ActivateReturnDTO> Activate(int MemberId,MemberRole Role,Plan plan);
        public Task<ActivateReturnDTO> Deactivate(int MemberId);
    }
}
