using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApi.Domain.Entities;
using BlogApi.Domain.Enums;
using BlogApi.Domain.Interfaces;
using BlogApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BlogApi.Infrastructure.Repositories
{    public class ArticleRepository : IArticleRepository
    {
        private readonly BlogDbContext _context;

        public ArticleRepository(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await _context.Articles
                .Include(a => a.Author)
                .ToListAsync();
        }

        public async Task<Article?> GetByIdAsync(int id)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Article> CreateAsync(Article article)
        {
            article.PublishedDate = DateTime.UtcNow;
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task<Article> UpdateAsync(Article article)
        {
            _context.Articles.Update(article);
            await _context.SaveChangesAsync();
            return article;
        }

        public async Task DeleteAsync(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Articles.AnyAsync(a => a.Id == id);
        }

        public async Task<(IEnumerable<Article> Articles, int TotalCount)> SearchAndFilterAsync(
            string? searchTerm = null,
            string? category = null,
            int? authorId = null,
            DateTime? publishedDateFrom = null,
            DateTime? publishedDateTo = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Articles
                .Include(a => a.Author)
                .AsQueryable();            // Search in title and content (case-insensitive)
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearchTerm = searchTerm.ToLower();
                query = query.Where(a => 
                    a.Title.ToLower().Contains(lowerSearchTerm) || 
                    a.Content.ToLower().Contains(lowerSearchTerm));
            }

            // Filter by category
            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(a => a.Category == category);
            }

            // Filter by author
            if (authorId.HasValue)
            {
                query = query.Where(a => a.AuthorId == authorId.Value);
            }

            // Filter by date range
            if (publishedDateFrom.HasValue)
            {
                query = query.Where(a => a.PublishedDate >= publishedDateFrom.Value);
            }

            if (publishedDateTo.HasValue)
            {
                query = query.Where(a => a.PublishedDate <= publishedDateTo.Value);
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply pagination and ordering
            var articles = await query
                .OrderByDescending(a => a.PublishedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (articles, totalCount);
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _context.Articles
                .Where(a => !string.IsNullOrEmpty(a.Category))
                .Select(a => a.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<Article?> GetBySlugAsync(string slug)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Slug == slug);
        }

        public async Task<bool> SlugExistsAsync(string slug, int? excludeId = null)
        {
            var query = _context.Articles.Where(a => a.Slug == slug);
            
            if (excludeId.HasValue)
            {
                query = query.Where(a => a.Id != excludeId.Value);
            }
            
            return await query.AnyAsync();
        }

        public async Task<IEnumerable<Article>> GetPublishedArticlesAsync()
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Where(a => a.Status == ArticleStatus.Published)
                .OrderByDescending(a => a.PublishedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetDraftsByUserAsync(int userId)
        {
            return await _context.Articles
                .Include(a => a.Author)
                .Where(a => a.AuthorId == userId && a.Status == ArticleStatus.Draft)
                .OrderByDescending(a => a.UpdatedAt)
                .ToListAsync();
        }
    }
}
