using System.ComponentModel.DataAnnotations;

namespace RefundManagementApplication.Models.Enums
{
    public enum RefundStatuses
    {
        [Display(Name = "Succeed")]
        SUCCEED = 2,
        [Display(Name = "Pending")]
        PENDING = 1,
        [Display(Name = "Failed")]
        FAILED = 0
    }
}
