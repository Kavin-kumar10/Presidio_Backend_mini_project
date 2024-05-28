using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RefundManagementApplication.Models.Enums
{
    public enum Plan
    {
        [Display(Name = "Free")]
        Free = 0,

        [Display(Name = "Prime")]
        Prime = 1
    }
}
