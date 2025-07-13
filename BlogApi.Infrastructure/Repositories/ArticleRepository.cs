using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infrastructure.Repositories
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly BlogDbContext _context;

        public ArticleRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task AddAsync(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
        }
    }
}
