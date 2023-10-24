using GreenSale.Persistence.Dtos.BuyerPostImageUpdateDtos;
using GreenSale.Persistence.Dtos.BuyerPostsDto;
using GreenSale.Persistence.Validators;
using GreenSale.Persistence.Validators.BuyerPosts;
using GreenSale.Service.Interfaces.BuyerPosts;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Admin.BuyerPosts;

[Route("api/admin/buyer/post")]
[ApiController]
public class AdminBuyerPostController : AdminBaseController
{
    private IBuyerPostService _service;

    public AdminBuyerPostController(IBuyerPostService service)
    {
        this._service = service;
    }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] BuyerPostCreateDto dto)
    {
        var validator = new BuyerPostCreateValidator();
        var isValidator = validator.Validate(dto);

        if (isValidator.IsValid)
        {
            return Ok(await _service.CreateAsync(dto));
        }

        return BadRequest(isValidator.Errors);
    }

    [HttpPut("{postId}")]
    public async Task<IActionResult> UpdateAsync([FromForm] BuyerPostUpdateDto dto, long postId)
    {
        var validator = new BuyerPostUpdateValidator();
        var isValidator = validator.Validate(dto);
        if (isValidator.IsValid)
        {
            return Ok(await _service.UpdateAsync(postId, dto));
        }

        return BadRequest(isValidator.Errors);
    }

    [HttpPut("status/{postId}")]
    public async Task<IActionResult> UpdateStatusAsync([FromForm] BuyerPostStatusUpdateDto dto, long postId)
        => Ok(await _service.UpdateStatusAsync(postId, dto));

    [HttpPut("image/{imageId}")]
    public async Task<IActionResult> ImageUpdateAsync(long imageId, [FromForm] BuyerPostImageDto dto)
    {
        var validator = new BuyerImageValidator();
        var isValidator = validator.Validate(dto);

        if (isValidator.IsValid)
        {
            var result = await _service.ImageUpdateAsync(imageId, dto);

            return Ok(result);
        }

        return BadRequest(isValidator.Errors);
    }

    [HttpDelete("{postId}")]
    public async Task<IActionResult> DeleteAsync(long postId)
        => Ok(await _service.DeleteAsync(postId));

    [HttpDelete("image/{imageId}")]
    public async Task<IActionResult> DeleteImageIdAsync(long imageId)
        => Ok(await _service.DeleteImageIdAsync(imageId));
}
