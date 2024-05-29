using RefundManagementApplication.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefundManagementApplication.Models
{
    public class Refund
    {
        public int RefundId { get; set; }
        public double RefundAmount { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Reason {  get; set; }
        public RefundStatuses RefundStatus { get; set; } = RefundStatuses.PENDING;

        public int CreatedBy { get; set; }
        public Member? CreatedByMember { get; set; }
        public int? ClosedBy { get; set; }
        public Member? ClosedByMember { get; set; }

        public int OrderId { get; set; }       
        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }
    }
}
