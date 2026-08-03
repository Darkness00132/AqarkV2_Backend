namespace Domain.Enums
{
    public enum AdAction
    {
        Delete,
        Update,
        Created
    }

    public static class AdActionExtensions
    {
        public static string ToArabic(this AdAction action) => action switch
        {
            AdAction.Delete => "حذف",
            AdAction.Update => "تحديث",
            AdAction.Created => "إنشاء",
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }
}