

namespace Application.DTOs.User
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;  // encoded token from URL
        public string NewPassword { get; set; } = string.Empty;

    }
}
