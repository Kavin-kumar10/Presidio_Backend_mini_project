using System.ComponentModel.DataAnnotations;

namespace RefundManagementApplication.Models.Enums
{
    public enum OrderStatuses
    {
        [Display(Name = "Ordered")]
        Ordered = 0,

        [Display(Name = "Refund Intiated")]
        Refund_Initiated = 1,

        [Display(Name = "Refund Rejected")]
        Refund_Rejected = 2,

        [Display(Name = "Refund Accepted")]
        Refund_Accepted = 3, // Product returned without damage by Collector

        [Display(Name = "Refund Complete")]
        Refund_Completed = 4
    }
}
