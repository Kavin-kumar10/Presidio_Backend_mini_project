using RefundManagementApplication.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace RefundManagementApplication.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public OrderStatuses OrderStatus { get; set; } = OrderStatuses.Ordered;


        [ForeignKey("ProductId")]
        public int ProductId { get; set; }
        public Product product { get; set; }

        [ForeignKey("RefundId")]
        public int? RefundId { get; set; }
        public Refund? Refund { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public double TotalPrice { get; set; }



        [ForeignKey("MemberId")]
        public int MemberID { get; set; }
        public Member OrderedBy { get; set; }

    }
}
