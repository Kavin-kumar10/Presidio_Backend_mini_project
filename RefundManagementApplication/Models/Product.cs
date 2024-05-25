using System.ComponentModel.DataAnnotations;

namespace RefundManagementApplication.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Act_price { get; set; }
        public float Curr_price { get; set; }
        public int Returnable { get; set; }
        public int ReturnableForPrime { get; set; }
        public int Count { get; set; }
    }
}
