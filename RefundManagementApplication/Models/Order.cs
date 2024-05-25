using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefundManagementApplication.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string OrderStatus { get; set; } = "Ordered";


        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product product { get; set; }


        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public double TotalPrice { get; set; }



        [ForeignKey("MemberId")]
        public int MemberID { get; set; }
        public Member OrderedBy { get; set; }



        [ForeignKey("RefundId")]
        public int? RefundId { get; set; }
        public Refund OrderRefund { get; set; }

    }
}
