using GreenSale.Application.Exceptions.Roles;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Roles;
using GreenSale.Domain.Entites.Roles;
using GreenSale.Persistence.Dtos.RoleDtos;
using GreenSale.Service.Helpers;
using GreenSale.Service.Interfaces.Common;
using GreenSale.Service.Interfaces.Roles;

namespace GreenSale.Service.Service.Roles;

public class RoleService : IRoleService
{
    private IRoleRepository _roleRepository;
    private IPaginator _paginator;

    public RoleService(
        IRoleRepository roleRepository,
        IPaginator paginator)
    {
        this._roleRepository = roleRepository;
        this._paginator = paginator;
    }

    public async Task<long> CountAsync()
    {
        return await _roleRepository.CountAsync();
    }

    public async Task<bool> CreateAsync(RoleCreatDto dto)
    {
        Role role = new Role()
        {
            Name = dto.Name,
            CreatedAt = TimeHelper.GetDateTime(),
            UpdatedAt = TimeHelper.GetDateTime()
        };

        var result = await _roleRepository.CreateAsync(role);

        return result > 0;
    }

    public async Task<bool> DeleteAsync(long roleId)
    {
        var roleres = await _roleRepository.GetByIdAsync(roleId);

        if (roleres.Id == 0)
            throw new RoleNotFoundException();

        var result = await _roleRepository.DeleteAsync(roleId);

        return result > 0;
    }

    public async Task<List<Role>> GetAllAsync(PaginationParams @params)
    {
        var rolesGet = await _roleRepository.GetAllAsync(@params);
        var count = await _roleRepository.CountAsync();
        _paginator.Paginate(count, @params);

        return rolesGet;
    }

    public async Task<Role> GetByIdAsync(long roleId)
    {
        var rolesGet = await _roleRepository.GetByIdAsync(roleId);

        if (rolesGet.Id == 0)
            throw new RoleNotFoundException();

        return rolesGet;
    }

    public async Task<bool> UpdateAsync(long roleId, RoleCreatDto dto)
    {
        var roleUpdate = await _roleRepository.GetByIdAsync(roleId);

        if (roleUpdate.Id == 0)
            throw new RoleNotFoundException();

        roleUpdate.Name = dto.Name;
        roleUpdate.UpdatedAt = TimeHelper.GetDateTime();
        var result = await _roleRepository.UpdateAsync(roleId, roleUpdate);

        return result > 0;
    }
}
