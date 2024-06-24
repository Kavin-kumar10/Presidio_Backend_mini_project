namespace RefundManagementApplication.Models.DTOs.RequestDTO.PaymentReqDTOs
{
    public class PaymentRequestDTO
    {
        public int AdminId { get; set; }
        public int RefundId {  get; set; }
        public Guid TransactionId { get; set; }
    }
}
