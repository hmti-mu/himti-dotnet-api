using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApi.Domain.Entities;

namespace BlogApi.Domain.Interfaces
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAllAsync();
        Task<Article> GetByIdAsync(int id);
        Task AddAsync(Article article);
    }
}
