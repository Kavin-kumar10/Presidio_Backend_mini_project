namespace RefundManagementApplication.Models.DTOs.ResponseDTO.LoginResponseDTOs
{
    public class LoginReturnDTO
    {
        public int MemberID { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
