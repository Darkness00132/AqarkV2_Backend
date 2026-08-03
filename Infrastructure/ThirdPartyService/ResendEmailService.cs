using Application.Interfaces.ThirdParty;
using Microsoft.Extensions.Configuration;
using Resend;

namespace Infrastructure.Services.ThirdPartyService
{
    public class ResendEmailService : IEmailService
    {
        private readonly IResend _resend;
        private readonly IConfiguration _config;

        public ResendEmailService(IResend resend, IConfiguration config)
        {
            _resend = resend;
            _config = config;
        }

        public async Task SendWelcomeEmailAsync(string to, string userName)
        {
            var message = new EmailMessage
            {
                From = "YourApp <no-reply@yourdomain.com>",
                //To = { to },
                To = "delivered@resend.dev",
                Subject = "Welcome!",
                HtmlBody = $"<h2>Welcome, {userName}!</h2><p>Glad to have you with us.</p>"
            };

            await _resend.EmailSendAsync(message);
        }

        public async Task SendValidationEmailAsync(string to, string token, Guid userId)
        {
            string frontendUrl = _config.GetValue<string>("FrontendUrl")!;
            string url = $"{frontendUrl}/verify-email?userId={userId}&token={token}";

            var message = new EmailMessage
            {
                From = "onboarding@resend.dev",
                //To = { to },
                To = "delivered@resend.dev",
                Subject = "Verify Your Email",
                HtmlBody = $"<p>Please verify your email:</p><a href='{url}'>{url}</a>"
            };

            await _resend.EmailSendAsync(message);
        }

        public async Task SendPasswordResetEmailAsync(string to, string resetLink)
        {
            var message = new EmailMessage
            {
                From = "YourApp <no-reply@yourdomain.com>",
                //To = { to },
                To = "delivered@resend.dev",
                Subject = "Reset Your Password",
                HtmlBody = $"<p>Click here to reset your password:</p><a href='{resetLink}'>{resetLink}</a>"
            };

            await _resend.EmailSendAsync(message);
        }
    }
}
