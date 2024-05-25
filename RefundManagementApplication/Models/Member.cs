using System.ComponentModel.DataAnnotations;

namespace RefundManagementApplication.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string email { get; set; }
        public string Role { get; set; } = "User";
        public string? Membership { get; set; } = "Free";
        public IList<Order> orders { get; set; }
    }
}


//Role -> Admin,User,Collector
