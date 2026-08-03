using Application.Interfaces.Ads;
using Domain.Entities.AdEntities;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Ads
{
    public class ImageRepo : IImageRepo
    {
        private readonly AppDbContext _context;

        public ImageRepo(AppDbContext context) => _context = context;

        public async Task AddRangeAsync(List<Image> images, CancellationToken ct = default)
        {
            await _context.Images.AddRangeAsync(images, ct);
        }

        /// <summary>
        /// get images of specific ad
        /// </summary>
        /// <param name="adId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task<List<Image>> GetByAdIdAsync(Guid adId, CancellationToken ct = default)
        {
            return await _context.Images
                .Where(i => i.AdId == adId)
                .ToListAsync(ct);
        }


        /// <summary>
        /// delete image by image id not ad id
        /// </summary>
        /// <param name="id">image</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var image = await _context.Images.FindAsync(id);
            if (image != null)
                _context.Images.Remove(image);
        }
    }
}
