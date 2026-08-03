using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class CompleteProfileDTO
    {

        [Required(ErrorMessage = "رقم الهاتف مطلوب")]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "صيغة رقم الهاتف خاطئة")]
        public required string PhoneNumber { get; set; }

        [Required(ErrorMessage = "دور مستخدم مطلوب")]
        public required PublicRoles Role { get; set; }
    }
}
