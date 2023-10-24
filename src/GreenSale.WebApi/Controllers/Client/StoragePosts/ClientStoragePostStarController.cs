using GreenSale.Persistence.Dtos.StoragDtos;
using GreenSale.Persistence.Validators.PostStar;
using GreenSale.Service.Interfaces.Storages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Client.StoragePosts
{
    [Route("api/client/storage/post/star")]
    [ApiController]
    public class ClientStoragePostStarController : BaseClientController
    {
        public readonly IStoragePostStarService _storagePostStarService;
        public ClientStoragePostStarController(IStoragePostStarService storagePostStarService)
        {
            this._storagePostStarService=storagePostStarService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] StorageStarCreateDto dto)
        {
            PostStarValidator validations = new PostStarValidator();
            var resltvalid = validations.Validate(dto.Stars);
            if (resltvalid.IsValid)
            {
                var result = await _storagePostStarService.CreateAsync(dto);

                return Ok(result);
            }
            else
                return BadRequest(resltvalid.Errors);
        }

        //[HttpPut("{postId}")]
        //public async Task<IActionResult> UpdateAsync([FromForm] long postId, [FromForm] StorageStarUpdateDto dto)
        //{
        //    PostStarValidator validations = new PostStarValidator();
        //    var resltvalid = validations.Validate(dto.Stars);
        //    if (resltvalid.IsValid)
        //    {
        //        var result = await _storagePostStarService.UpdateAsync(postId, dto);

        //        return Ok(result);
        //    }
        //    else
        //        return BadRequest(resltvalid.Errors);
        //}
    }
}
