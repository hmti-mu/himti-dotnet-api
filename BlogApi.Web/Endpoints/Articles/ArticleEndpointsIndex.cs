// Articles Domain - Endpoint Index
// This file serves as documentation for all Article-related endpoints

using BlogApi.Application.DTOs;
using BlogApi.Web.Models.Requests;

namespace BlogApi.Web.Endpoints.Articles
{
    /// <summary>
    /// Articles Domain Endpoints
    /// Base Route: /api/articles
    /// Tag: Articles
    /// 
    /// Available Endpoints:
    /// - GET    /api/articles       -> GetArticlesEndpoint
    /// - POST   /api/articles       -> CreateArticleEndpoint  (Auth: User)
    /// - GET    /api/articles/{id}  -> GetArticleByIdEndpoint
    /// - PUT    /api/articles/{id}  -> UpdateArticleEndpoint  (Auth: User)
    /// - DELETE /api/articles/{id}  -> DeleteArticleEndpoint  (Auth: User)
    /// </summary>
    public static class ArticleEndpointsIndex
    {
        public const string Tag = "Articles";
        public const string BaseRoute = "/api/articles";
        
        // Endpoint Types
        public static readonly Type GetAllEndpoint = typeof(GetArticlesEndpoint);
        public static readonly Type CreateEndpoint = typeof(CreateArticleEndpoint);
        public static readonly Type GetByIdEndpoint = typeof(GetArticleByIdEndpoint);
        public static readonly Type UpdateEndpoint = typeof(UpdateArticleEndpoint);
        public static readonly Type DeleteEndpoint = typeof(DeleteArticleEndpoint);
        
        // Request/Response Types
        public static readonly Type ResponseDto = typeof(ArticleDto);
        public static readonly Type CreateRequest = typeof(CreateArticleRequest);
        public static readonly Type UpdateRequest = typeof(UpdateArticleRequestModel);
    }
}

