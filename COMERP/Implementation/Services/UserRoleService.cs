using COMERP.Abstractions.Interfaces;

namespace COMERP.Implementation.Services
{
    public class UserRoleService : IUserRoleService
    {
        public Task<bool> AssignUserToRole(string userName, IList<string> roles)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> GetUserRolesAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(string userId, string role)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUsersRole(string userName, IList<string> usersRole)
        {
            throw new NotImplementedException();
        }
    }
}
