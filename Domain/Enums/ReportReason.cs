namespace Domain.Enums
{
    public enum ReportReason
    {
        Fraud,
        FakeListing,
        Harassment,
        Other
    }

    public static class ReportReasonExtensions
    {
        public static string ToArabic(this ReportReason reason) => reason switch
        {
            ReportReason.Fraud => "احتيال",
            ReportReason.FakeListing => "إعلان مزيف",
            ReportReason.Harassment => "مضايقة",
            ReportReason.Other => "أخرى",
            _ => throw new ArgumentOutOfRangeException(nameof(reason), reason, null)
        };
    }
}