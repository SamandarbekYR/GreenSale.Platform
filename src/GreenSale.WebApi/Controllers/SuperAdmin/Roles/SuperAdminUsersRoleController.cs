using GreenSale.Application.Utils;
using GreenSale.Persistence.Dtos.RoleDtos;
using GreenSale.Service.Interfaces.Roles;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.SuperAdmin.Roles;

[Route("api/superadmin/user/roles")]
[ApiController]
public class SuperAdminUsersRoleController : SuperAdminBaseController
{
    private readonly IUserRoleService _service;
    private int maxPage = 30;

    public SuperAdminUsersRoleController(IUserRoleService userRoleService)
    {
        this._service = userRoleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1)
        => Ok(await _service.GetAllAsync(new PaginationParams(page, maxPage)));

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetByIdAsync(long Id)
        => Ok(await _service.GetByIdAsync(Id));

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync()
        => Ok(await _service.CountAsync());

    [HttpPut("{Id}")]
    public async Task<IActionResult> UpdateAsync(long Id, UserRoleDtoUpdate dto)
        => Ok(await _service.UpdateAsync(Id, dto));

    [HttpDelete("{Id}")]
    public async Task<IActionResult> DeleteAsync(long Id)
        => Ok(await _service.DeleteAsync(Id));
}
