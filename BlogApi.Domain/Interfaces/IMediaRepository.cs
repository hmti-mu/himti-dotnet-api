using BlogApi.Domain.Entities;

namespace BlogApi.Domain.Interfaces
{
    public interface IMediaRepository
    {
        Task<IEnumerable<Media>> GetAllAsync(int page = 1, int pageSize = 10);
        Task<Media?> GetByIdAsync(int id);
        Task<Media> CreateAsync(Media media);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<(IEnumerable<Media> Media, int TotalCount)> GetByUserIdAsync(int userId, int page = 1, int pageSize = 10);
    }
}