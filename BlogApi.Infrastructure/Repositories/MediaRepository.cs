using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infrastructure.Repositories
{
    public class MediaRepository : IMediaRepository
    {
        private readonly BlogDbContext _context;

        public MediaRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Media>> GetAllAsync(int page = 1, int pageSize = 10)
        {
            return await _context.Media
                .Include(m => m.UploadedBy)
                .OrderByDescending(m => m.UploadedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Media?> GetByIdAsync(int id)
        {
            return await _context.Media
                .Include(m => m.UploadedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Media> CreateAsync(Media media)
        {
            _context.Media.Add(media);
            await _context.SaveChangesAsync();
            return media;
        }

        public async Task DeleteAsync(int id)
        {
            var media = await _context.Media.FindAsync(id);
            if (media != null)
            {
                _context.Media.Remove(media);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Media.AnyAsync(m => m.Id == id);
        }

        public async Task<(IEnumerable<Media> Media, int TotalCount)> GetByUserIdAsync(int userId, int page = 1, int pageSize = 10)
        {
            var query = _context.Media
                .Include(m => m.UploadedBy)
                .Where(m => m.UploadedByUserId == userId);

            var totalCount = await query.CountAsync();
            
            var media = await query
                .OrderByDescending(m => m.UploadedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (media, totalCount);
        }
    }
}