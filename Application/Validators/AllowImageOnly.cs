using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Validators
{
    public class AllowImageOnly : ValidationAttribute
    {
        private readonly string[] _extensions = { ".jpg", ".jpeg", ".png", ".webp" };

        private static readonly Dictionary<string, byte[]> _signatures = new()
        {
            { ".jpg",  new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".jpeg", new byte[] { 0xFF, 0xD8, 0xFF } },
            { ".png",  new byte[] { 0x89, 0x50, 0x4E, 0x47 } },
            { ".webp", new byte[] { 0x52, 0x49, 0x46, 0x46 } },
        };

        public int MinCount { get; init; } = 1;
        public int MaxCount { get; init; } = 5;

        protected override ValidationResult? IsValid(object? value, ValidationContext ctx)
        {
            if (value is not IFormFile[] files)
                return ValidationResult.Success;

            if (files.Length < MinCount)
                return new ValidationResult("يجب ارفاق صورة واحدة على الاقل");

            if (files.Length > MaxCount)
                return new ValidationResult($"عدد الصور المرفقة لا يجب ان يتجاوز ال {MaxCount} صور");

            foreach (var file in files)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

                if (!_extensions.Contains(extension))
                    return new ValidationResult("صيغة الصورة غير مدعومة، يُسمح فقط بـ JPG و PNG و WebP");

                if (!IsValidSignature(file, extension))
                    return new ValidationResult("الملف المرفق ليس صورة حقيقية");
            }

            return ValidationResult.Success;
        }

        private bool IsValidSignature(IFormFile file, string extension)
        {
            if (!_signatures.TryGetValue(extension, out var signature)) return false;
            using var reader = new BinaryReader(file.OpenReadStream());
            var bytes = reader.ReadBytes(signature.Length);
            return bytes.SequenceEqual(signature);
        }
    }
}