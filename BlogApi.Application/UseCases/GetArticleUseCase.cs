using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApi.Application.DTOs;
using BlogApi.Domain.Interfaces;

namespace BlogApi.Application.UseCases
{
    public class GetArticlesUseCase
    {
        private readonly IArticleRepository _articleRepository;

        public GetArticlesUseCase(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public async Task<IEnumerable<ArticleDto>> ExecuteAsync()
        {
            var articles = await _articleRepository.GetAllAsync();
            var articleDtos = new List<ArticleDto>();
            foreach (var article in articles)
            {
                articleDtos.Add(
                    new ArticleDto
                    {
                        Id = article.Id,
                        Title = article.Title,
                        Content = article.Content.ToString(),
                        PublishedDate = article.PublishedDate,
                    }
                );
            }
            return articleDtos;
        }
    }
}
