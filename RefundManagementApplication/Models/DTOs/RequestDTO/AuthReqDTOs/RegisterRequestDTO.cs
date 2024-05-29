using System.ComponentModel.DataAnnotations;

namespace RefundManagementApplication.Models.DTOs.RequestDTO.AuthReqDTOs
{
    public class RegisterRequestDTO
    {
        [Required(ErrorMessage = "User Name cannot be empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Password Cannot be empty")]
        [Range(50, 100, ErrorMessage = "Password is Too Short")]
        public string password { get; set; }

        [Required(ErrorMessage = "EmailAddress Cannot be empty")]
        [EmailAddress]
        public string email { get; set; }
    }
}
