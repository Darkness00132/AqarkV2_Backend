namespace Application.Interfaces.ThirdParty
{
    public interface IStorageService
    {
        Task<string> UploadOneAsync(Stream image, CancellationToken ct = default, bool IsAvatar = false);
        Task<List<string>> UploadManyAsync(IEnumerable<Stream> images, CancellationToken ct = default);
        Task DeleteOneAsync(string url, CancellationToken ct = default);
        Task DeleteManyAsync(IEnumerable<string> urls, CancellationToken ct = default);
    }
}
