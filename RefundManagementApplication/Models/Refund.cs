namespace RefundManagementApplication.Models
{
    public class Refund
    {
        public int RefundId { get; set; }
        public double RefundAmount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Reason {  get; set; }
        public string RefundStatus { get; set; }
        public int? PaymentId { get; set; }
        public int CreatedBy { get; set; }
        public Member CreatedByMember { get; set; }
        public int ClosedBy { get; set; }
        public int ClosedByMember { get; set; }
    }
}
