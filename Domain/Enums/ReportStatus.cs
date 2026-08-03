namespace Domain.Enums
{
    public enum ReportStatus
    {
        Pending,
        UnderReview,
        Resolved,
        Dismissed
    }

    public static class ReportStatusExtensions
    {
        public static string ToArabic(this ReportStatus status) => status switch
        {
            ReportStatus.Pending => "قيد الانتظار",
            ReportStatus.UnderReview => "قيد المراجعة",
            ReportStatus.Resolved => "تم الحل",
            ReportStatus.Dismissed => "مرفوض",
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };
    }
}