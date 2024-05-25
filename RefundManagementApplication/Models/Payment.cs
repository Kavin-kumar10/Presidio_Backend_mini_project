namespace RefundManagementApplication.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public int RefundId { get; set; }
        public string Type { get; set; }
        public double TotalPayment { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
