using RefundManagementApplication.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RefundManagementApplication.Models.DTOs.RequestDTO.ActivationReqDTOs
{
    public class ActivationReqDTO
    {
        public int MemberId { get; set; }
        public MemberRole Role { get; set; }
        public Plan Plan { get; set; }
    }
}
