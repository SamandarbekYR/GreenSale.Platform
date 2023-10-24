using GreenSale.Application.Utils;
using GreenSale.Domain.Entites.Roles;
using GreenSale.Persistence.Dtos.RoleDtos;

namespace GreenSale.Service.Interfaces.Roles;

public interface IRoleService
{
    public Task<bool> CreateAsync(RoleCreatDto dto);
    public Task<bool> UpdateAsync(long roleId, RoleCreatDto dto);
    public Task<bool> DeleteAsync(long roleId);
    public Task<List<Role>> GetAllAsync(PaginationParams @params);
    public Task<Role> GetByIdAsync(long roleId);
    public Task<long> CountAsync();
}
