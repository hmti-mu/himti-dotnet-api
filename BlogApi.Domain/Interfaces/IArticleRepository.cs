using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApi.Domain.Entities;

namespace BlogApi.Domain.Interfaces
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article?> GetByIdAsync(int id);
        Task<Article> CreateAsync(Article article);
        Task<Article> UpdateAsync(Article article);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<(IEnumerable<Article> Articles, int TotalCount)> SearchAndFilterAsync(
            string? searchTerm = null,
            string? category = null,
            int? authorId = null,
            DateTime? publishedDateFrom = null,
            DateTime? publishedDateTo = null,
            int page = 1,
            int pageSize = 10);
        Task<IEnumerable<string>> GetCategoriesAsync();
    }
}
