namespace Application.Interfaces.ThirdParty
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string to, string userName);

        Task SendValidationEmailAsync(string to, string token, Guid userId);

        Task SendPasswordResetEmailAsync(string to, string resetLink);
    }
}
