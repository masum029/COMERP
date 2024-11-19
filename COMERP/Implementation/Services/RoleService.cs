using COMERP.Abstractions.Interfaces;
using COMERP.DTOs;
using COMERP.Entities;
using COMERP.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace COMERP.Implementation.Services
{
    public class RoleService : IRoleService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public RoleService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new ApplicationRole(roleName));
            if (!result.Succeeded)
            {
                throw new NotFoundException($"Role Crated faild : {result.Errors}");
            }
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(string roleId)
        {
            var roleDetails = await _roleManager.FindByIdAsync(roleId);
            if (roleDetails == null)
            {
                throw new NotFoundException("Role not found");
            }

            if (roleDetails.Name == "Administrator")
            {
                throw new BadRequestException("You can not delete Administrator Role");
            }
            var result = await _roleManager.DeleteAsync(roleDetails);
            if (!result.Succeeded)
            {
                throw new NotFoundException($"Role Crated faild : {result.Errors}");
            }
            return result.Succeeded;
        }

        public async Task<ApplicationRoleDTO> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                throw new NotFoundException($"{nameof(role)} does not exist");
            }
            var result = new ApplicationRoleDTO()
            {
                Id = role.Id,
                RoleName = role.Name,
            };
            return result;
        }

        public async Task<IEnumerable<ApplicationRoleDTO>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles.Select(x => new ApplicationRoleDTO
            {
                RoleName = x.Name,
                Id = x.Id
            }).ToListAsync();

            return roles;
        }
        public async Task<bool> UpdateRole(string id, string roleName)
        {
            if (roleName != null)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    throw new NotFoundException(" Role does not exist");
                }
                role.Name = roleName;
                var result = await _roleManager.UpdateAsync(role);
                return result.Succeeded;
            }
            return false;
        }
    }
}
