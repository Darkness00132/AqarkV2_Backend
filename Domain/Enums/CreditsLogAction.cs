namespace Domain.Enums
{
    public enum CreditsLogAction
    {
        Purchase,
        Spend,
        Refund,
        Gift
    }

    public static class CreditsLogActionExtensions
    {
        public static string ToArabic(this CreditsLogAction action) => action switch
        {
            CreditsLogAction.Purchase => "شراء",
            CreditsLogAction.Spend => "إنفاق",
            CreditsLogAction.Refund => "استرداد",
            CreditsLogAction.Gift => "هدية",
            _ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
        };
    }
}