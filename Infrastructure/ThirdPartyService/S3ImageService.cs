using Amazon.S3;
using Amazon.S3.Model;
using Application.Interfaces.ThirdParty;
using Application.Options;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Infrastructure.Services.ThirdPartyService
{
    public class S3ImageService : IStorageService
    {
        private readonly IAmazonS3 _s3;
        private readonly S3Settings _s3Settings;
        public S3ImageService(IAmazonS3 S3, IOptionsSnapshot<S3Settings> options)
        {
            _s3 = S3;
            _s3Settings = options.Value;
        }
        public async Task DeleteManyAsync(IEnumerable<string> urls, CancellationToken ct = default)
        {
            List<string> keys = urls.Select(url => GetKeyFromUrl(url)).ToList();

            if (keys.Count == 0) return;

            await _s3.DeleteObjectsAsync(new DeleteObjectsRequest
            {
                BucketName = _s3Settings.BucketName,
                Objects = keys.Select(key => new KeyVersion { Key = key }).ToList()
            }, ct);
        }

        public async Task DeleteOneAsync(string url, CancellationToken ct = default)
        {
            string key = GetKeyFromUrl(url);

            ct.ThrowIfCancellationRequested();
            await _s3.DeleteObjectAsync(new DeleteObjectRequest
            {
                BucketName = _s3Settings.BucketName,
                Key = key
            }, ct);
        }

        public async Task<List<string>> UploadManyAsync(IEnumerable<Stream> images, CancellationToken ct = default)
        {
            List<string> PublicUrls = new List<string>();

            try
            {
                foreach (Stream image in images)
                {
                    string KeyName = $"ads/{Guid.NewGuid():N}.webp";
                    ValidateStream(image);
                    using var WebpImage = await ConvertToWebpAsync(image);
                    await _s3.PutObjectAsync(new PutObjectRequest
                    {
                        BucketName = _s3Settings.BucketName,
                        Key = KeyName,
                        InputStream = WebpImage,
                        ContentType = "image/webp",
                        Headers = { CacheControl = "public, max-age=31536000, immutable" }
                    }, ct);
                    PublicUrls.Add(GetPublicUrl(KeyName));
                }

                return PublicUrls;
            }
            catch (Exception ex)
            {
                await this.DeleteManyAsync(PublicUrls, ct);
                throw new Exception($"Failed to upload all images. Rolled back any uploaded images. Reason: {ex.Message}", ex);
            }
        }

        public async Task<string> UploadOneAsync(Stream image, CancellationToken ct = default, bool IsAvatar = false)
        {
            string KeyName = IsAvatar
            ? $"avatar/{Guid.NewGuid():N}.webp"
            : $"profileCovers/{Guid.NewGuid():N}.webp";

            ValidateStream(image);
            using var WebpImage = await ConvertToWebpAsync(image, IsAvatar);

            await _s3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _s3Settings.BucketName,
                Key = KeyName,
                InputStream = WebpImage,
                ContentType = "image/webp",
                Headers = { CacheControl = "public, max-age=31536000, immutable" }
            }, ct);

            return GetPublicUrl(KeyName);
        }

        private string GetPublicUrl(string key)
        {
            return $"https://{_s3Settings.BucketName}.s3.amazonaws.com/{key}";
        }

        private string GetKeyFromUrl(string url)
        {
            Uri uri = new Uri(url);
            return uri.AbsolutePath.TrimStart('/');
        }

        private void ValidateStream(Stream content)
        {
            if (content == null || !content.CanRead) throw new ArgumentException("Content stream is not readable.", nameof(content));
            if (content.CanSeek) content.Position = 0;
        }

        private async Task<MemoryStream> ConvertToWebpAsync(Stream stream, bool IsAvatar = false)
        {
            using var image = await Image.LoadAsync(stream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = IsAvatar ? new Size(256, 256) : new Size(1200, 0),
                Mode = IsAvatar ? ResizeMode.Crop : ResizeMode.Max,
                Position = AnchorPositionMode.Center,
                Sampler = KnownResamplers.Lanczos3      // sharp, high quality
            }
            ));

            var encoder = new WebpEncoder
            {
                Quality = IsAvatar ? 75 : 90,
                Method = IsAvatar ? WebpEncodingMethod.Fastest : WebpEncodingMethod.BestQuality,
                NearLossless = IsAvatar
            };

            var outputStream = new MemoryStream();
            await image.SaveAsync(outputStream, encoder);
            outputStream.Position = 0;
            return outputStream;
        }
    }
}
