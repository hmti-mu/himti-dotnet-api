using BlogApi.Domain.Entities;

namespace BlogApi.Domain.Interfaces
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(string name);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<IEnumerable<Role>> GetUserRolesAsync(int userId);
        Task AddUserToRoleAsync(int userId, int roleId);
        Task RemoveUserFromRoleAsync(int userId, int roleId);
        Task<bool> UserHasRoleAsync(int userId, string roleName);
        Task<int> GetUserHighestRoleLevelAsync(int userId);
    }
}