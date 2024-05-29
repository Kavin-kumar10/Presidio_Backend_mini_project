using System.ComponentModel.DataAnnotations;

namespace RefundManagementApplication.Models.DTOs.RequestDTO.OrderReqDTOs
{
    public class OrderRequestDTO
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        public int MemberID { get; set; }
    }
}
