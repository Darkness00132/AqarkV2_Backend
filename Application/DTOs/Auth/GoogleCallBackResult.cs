namespace Application.DTOs.User
{
    public class GoogleCallbackResult
    {
        public bool Success { get; set; }
        public string RedirectUrl { get; set; }
        public string AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public Guid? UserId { get; set; }

        public static GoogleCallbackResult Failed(string redirectUrl)
        {
            return new GoogleCallbackResult
            {
                Success = false,
                RedirectUrl = redirectUrl
            };
        }


        public static GoogleCallbackResult Successed(
            Guid userId,
            string refreshToken,
            DateTime expiresAt,
            string redirectUrl)
        {
            return new GoogleCallbackResult
            {
                UserId = userId,
                Success = true,
                RefreshToken = refreshToken,
                RedirectUrl = redirectUrl
            };

        }
    }
}
