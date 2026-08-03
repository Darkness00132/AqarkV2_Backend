using Application.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.User
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "اسم مستخدم مطلوب")]
        [StringLength(100, ErrorMessage = "الاسم طويل جدًا")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "الايميل مطلوب")]
        [EmailAddress(ErrorMessage = "صيغة الايميل خاطئة")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "صيغة رقم الهاتف خاطئة")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "كلمة المرور مطلوبة")]
        [MinLength(6, ErrorMessage = "كلمة المرور يجب أن تكون على الأقل 6 أحرف")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W).+$",
        ErrorMessage = "كلمة المرور يجب أن تحتوي على حرف صغير، حرف كبير، رقم، ورمز")]
        public required string Password { get; set; }

        [Required(ErrorMessage = "دور مستخدم مطلوب")]
        public required PublicRoles Role { get; set; }
    }
}
