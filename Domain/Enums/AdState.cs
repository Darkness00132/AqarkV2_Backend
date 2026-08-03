namespace Domain.Enums
{
    public enum AdState
    {
        Unfinished,
        HalfFinished,
        FullyFinished,
        Furnished,
    }

    public static class AdStateExtensions
    {
        public static string ToArabic(this AdState adState) => adState switch
        {
            AdState.Unfinished => "على الهيكل",
            AdState.HalfFinished => "نصف تشطيب",
            AdState.FullyFinished => "تشطيب كامل",
            AdState.Furnished => "مفروش",
            _ => throw new ArgumentOutOfRangeException(nameof(adState), adState, null)
        };
    }
}