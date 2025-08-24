using BlogApi.Application.DTOs;
using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class SearchArticlesUseCase
    {
        private readonly IArticleRepository _articleRepository;

        public SearchArticlesUseCase(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<ArticleSearchResultDto> ExecuteAsync(ArticleSearchFilterDto filter)
        {
            var (articles, totalCount) = await _articleRepository.SearchAndFilterAsync(
                searchTerm: filter.SearchTerm,
                category: filter.Category,
                authorId: filter.AuthorId,
                publishedDateFrom: filter.PublishedDateFrom,
                publishedDateTo: filter.PublishedDateTo,
                page: filter.Page,
                pageSize: filter.PageSize
            );

            var articleDtos = articles.Select(article => new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Category = article.Category,
                ThumbnailUrl = article.ThumbnailUrl,
                PublishedDate = article.PublishedDate,
                AuthorName = article.Author?.Username,
                AuthorId = article.AuthorId
            }).ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

            return new ArticleSearchResultDto
            {
                Articles = articleDtos,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize,
                TotalPages = totalPages
            };
        }
    }
}
