using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RefundManagementApplication.Models
{
    public class User
    {
        [Key]
        public int MemberId { get; set; }
        public Byte[] Password { get; set; }
        public Byte[] PasswordHashKey { get; set; }

        [ForeignKey("MemberId")]
        public Member Member { get; set; }
        public string Status { get; set; }

    }
}
