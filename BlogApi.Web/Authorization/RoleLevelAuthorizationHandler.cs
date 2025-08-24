using Microsoft.AspNetCore.Authorization;

namespace BlogApi.Web.Authorization
{
    public class MinimumRoleLevelRequirement : IAuthorizationRequirement
    {
        public int MinimumLevel { get; }

        public MinimumRoleLevelRequirement(int minimumLevel)
        {
            MinimumLevel = minimumLevel;
        }
    }

    public class MinimumRoleLevelHandler : AuthorizationHandler<MinimumRoleLevelRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MinimumRoleLevelRequirement requirement)
        {
            var user = context.User;

            if (!user.Identity?.IsAuthenticated == true)
            {
                return Task.CompletedTask;
            }

            var roleLevelClaims = user.Claims
                .Where(c => c.Type == "role_level")
                .Select(c => int.TryParse(c.Value, out var level) ? level : -1)
                .Where(level => level >= 0);

            var userMaxLevel = roleLevelClaims.Any() ? roleLevelClaims.Max() : -1;

            if (userMaxLevel >= requirement.MinimumLevel)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}