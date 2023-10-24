using GreenSale.Application.Utils;
using GreenSale.Service.Interfaces.Storages;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Common.Storages;

[Route("api/common/storage")]
[ApiController]
public class CommonStoragesController : BaseController
{
    private IStoragesService _service;
    private int maxPageSize = 20;

    public CommonStoragesController(IStoragesService service)
    {
        this._service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1)
        => Ok(await _service.GetAllAsync(new PaginationParams(page, maxPageSize)));

    [HttpGet("all/{userId}")]
    public async Task<IActionResult> GetAllByIdAsync(long userId, [FromQuery] int page = 1)
        => Ok(await _service.GetAllByIdAsync(userId, new PaginationParams(page, maxPageSize)));

    [HttpGet("{storageId}")]
    public async Task<IActionResult> GetByIdAsync(long storageId)
        => Ok(await _service.GetBYIdAsync(storageId));

    [HttpGet("count")]
    public async Task<IActionResult> CountAsync()
        => Ok(await _service.CountAsync());

    [HttpGet("search/info")]
    public async Task<IActionResult> SearchingAsync(string search)
    {
        var res = await _service.SearchAsync(search);

        return Ok(new { res.IteamCount, res.Item2 });
    }

    [HttpGet("created/daylily/count")]
    public async Task<IActionResult> GetCreatedDaylilyAsync([FromQuery] int day)
           => Ok(await _service.StorageDaylilyCreatedAsync(day));

    [HttpGet("created/monthly/count")]
    public async Task<IActionResult> GetCreatedMonthlyAsync([FromQuery] int month)
        => Ok(await _service.StorageMonthlyCreatedAsync(month));
}
