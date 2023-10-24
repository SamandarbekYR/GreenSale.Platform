using GreenSale.Persistence.Dtos.StoragDtos;
using GreenSale.Persistence.Validators;
using GreenSale.Persistence.Validators.Storages;
using GreenSale.Service.Interfaces.Storages;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Client.Storages;

[Route("api/client/storages")]
[ApiController]
public class ClientStoragePostController : BaseClientController
{
    private IStoragesService _service;
    private int maxPageSize = 30;

    public ClientStoragePostController(IStoragesService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] StoragCreateDto dto)
    {
        var validator = new StorageCreateValidator();
        var valid = await validator.ValidateAsync(dto);

        if (valid.IsValid)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        return BadRequest(valid.Errors);
    }


    [HttpPut("{storageId}")]
    public async Task<IActionResult> UpdateAsync([FromForm] StoragUpdateDto dto, long storageId)
    {
        var validator = new StorageUpdateValidator();
        var valid = await validator.ValidateAsync(dto);
        if (valid.IsValid)
        {
            return Ok(await _service.UpdateAsync(storageId, dto));
        }

        return BadRequest(valid.Errors);
    }

    [HttpPut("image/{storageId}")]
    public async Task<IActionResult> UpdateImageIdAsync([FromForm] StorageImageUpdateDto dto, long storageId)
    {
        var validator = new StorageValidatorDto();
        var isValidator = validator.Validate(dto);
        if (isValidator.IsValid)
        {
            return Ok(await _service.UpdateImageAsync(storageId, dto));
        }

        return BadRequest(isValidator.Errors);
    }

    [HttpDelete("{storageId}")]
    public async Task<IActionResult> DeleteAsync(long storageId)
        => Ok(await _service.DeleteAsync(storageId));
}
