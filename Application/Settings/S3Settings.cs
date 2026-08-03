namespace Application.Options
{
    public class S3Settings
    {
        public string Region { get; init; } = string.Empty;
        public string BucketName { get; init; } = string.Empty;
        public string? AccessKey { get; init; }
        public string? SecretKey { get; init; }
    }
}
