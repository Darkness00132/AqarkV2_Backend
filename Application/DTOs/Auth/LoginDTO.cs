using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "الايميل مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة الايميل خاطئة")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 6 أحرف")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف صغير، حرف كبير، رقم، ورمز")]
        public required string Password { get; set; }
    }
}
