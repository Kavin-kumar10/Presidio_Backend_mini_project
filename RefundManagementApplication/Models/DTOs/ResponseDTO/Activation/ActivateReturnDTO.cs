using RefundManagementApplication.Models.Enums;

namespace RefundManagementApplication.Models.DTOs.ResponseDTO.Activation
{
    public class ActivateReturnDTO
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public MemberRole Role { get; set; }
    }
}
