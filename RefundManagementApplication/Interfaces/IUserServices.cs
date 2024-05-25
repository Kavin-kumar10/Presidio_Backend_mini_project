using RefundManagementApplication.Models;
using RefundManagementApplication.Models.DTOs.RequestDTO.AuthReqDTOs;
using RefundManagementApplication.Models.DTOs.ResponseDTO.LoginResponseDTOs;

namespace RefundManagementApplication.Interfaces
{
    public interface IUserServices
    {
        public Task<LoginReturnDTO> Login(UserLoginDTO loginDTO);
        public Task<Member> Register(RegisterRequestDTO registerRequestDTO);
    }
}
