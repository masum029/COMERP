using COMERP.DTOs;

namespace COMERP.Abstractions.Interfaces
{
    public interface IRoleService
    {
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(string roleId);
        Task<IEnumerable<ApplicationRoleDTO>> GetRolesAsync();
        Task<ApplicationRoleDTO> GetRoleByIdAsync(string id);
        Task<bool> UpdateRole(string id, string roleName);
    }
}
