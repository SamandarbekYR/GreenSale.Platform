using GreenSale.Application.Utils;
using GreenSale.Persistence.Dtos.RoleDtos;
using GreenSale.Persistence.Validators.Roles;
using GreenSale.Service.Interfaces.Roles;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.SuperAdmin.Roles;

[Route("api/superadmin/roles")]
[ApiController]
public class SuperAdminRolesController : SuperAdminBaseController
{
    public int maxPage = 30;
    private readonly IRoleService _service;

    public SuperAdminRolesController(IRoleService roleService)
    {
        this._service = roleService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1)
        => Ok(await _service.GetAllAsync(new PaginationParams(page, maxPage)));

    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetByIdAsync(long roleId)
        => Ok(await _service.GetByIdAsync(roleId));

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync()
        => Ok(await _service.CountAsync());

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] RoleCreatDto dto)
    {
        RoleValidator validationRules = new RoleValidator();
        var result = validationRules.Validate(dto);

        if (result.IsValid)
            return Ok(await _service.CreateAsync(dto));

        return BadRequest(result.Errors);
    }

    [HttpPut("{roleId}")]
    public async Task<IActionResult> UpdateAsync(long roleId, RoleCreatDto dto)
    {
        RoleValidator validationRules = new RoleValidator();
        var result = validationRules.Validate(dto);

        if (result.IsValid)
            return Ok(await _service.UpdateAsync(roleId, dto));

        return BadRequest(result.Errors);
    }

    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteAsync(long roleId)
        => Ok(await _service.DeleteAsync(roleId));
}
