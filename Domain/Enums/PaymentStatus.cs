namespace Domain.Enums
{
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public static class PaymentStatusExtensions
    {
        public static string ToArabic(this PaymentStatus status) => status switch
        {
            PaymentStatus.Pending => "قيد الانتظار",
            PaymentStatus.Completed => "مكتمل",
            PaymentStatus.Failed => "فشل",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}