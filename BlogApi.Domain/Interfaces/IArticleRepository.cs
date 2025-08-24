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
    }
}
