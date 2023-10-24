using GreenSale.Application.Utils;
using GreenSale.DataAccess.ViewModels.UserRoles;
using GreenSale.Domain.Entites.Roles.UserRoles;

namespace GreenSale.DataAccess.Interfaces.Roles
{
    public interface IUserRoles : IRepository<UserRole, UserRoleViewModel>
    { 
        public Task<List<long>> GetAdminIdASync(PaginationParams @params);
        public Task<List<long>> GetUserIdASync(PaginationParams @params);
        public Task<UserRole>GetUserRole(long id);
    }
}