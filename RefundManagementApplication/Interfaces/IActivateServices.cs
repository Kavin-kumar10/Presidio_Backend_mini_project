using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.ResponseDTO.Activation;

namespace RefundManagementApplication.Interfaces
{
    public interface IActivateServices
    {
        public Task<ActivateReturnDTO> Activate(int MemberId);
        public Task<ActivateReturnDTO> Deactivate(int MemberId);
    }
}
