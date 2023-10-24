using GreenSale.Application.Utils;
using GreenSale.DataAccess.ViewModels.UserRoles;
using GreenSale.Domain.Entites.Roles.UserRoles;
using GreenSale.Persistence.Dtos.RoleDtos;

namespace GreenSale.Service.Interfaces.Roles
{
    public interface IUserRoleService
    {
        public Task<bool> UpdateAsync(long UserroleId, UserRoleDtoUpdate dto);
        public Task<bool> DeleteAsync(long UserroleId);
        public Task<List<UserRoleViewModel>> GetAllAsync(PaginationParams @params);
        public Task<UserRoleViewModel> GetByIdAsync(long UserroleId);
        public Task<long> CountAsync();
        public Task<UserRole> GetRole();
    }
}
