using System.ComponentModel.DataAnnotations;

namespace RefundManagementApplication.Models.Enums
{
    public enum MemberRole
    {
        [Display(Name = "User")]
        User = 0,

        [Display(Name = "Collector")]
        Collector = 1,

        [Display(Name = "Admin")]
        Admin = 2
    }
}
