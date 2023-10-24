using GreenSale.Persistence.Dtos.BuyerPostsDto;
using GreenSale.Persistence.Validators.PostStar;
using GreenSale.Service.Interfaces.BuyerPosts;
using GreenSale.WebApi.Controllers.Client;
using GreenSaleuz.Persistence.Validators.Dtos.AuthUserValidators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Admin.BuyerPosts
{
    [Route("api/admin/buyer/star")]
    [ApiController]
    public class AdminBuyerPostStarController : AdminBaseController
    {
        private readonly IBuyerPostStarService _buyerPostStarService;
        public AdminBuyerPostStarController(IBuyerPostStarService buyerPostStarService)
        {
            _buyerPostStarService = buyerPostStarService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] BuyerPostStarCreateDto dto)
        {
            PostStarValidator validations = new PostStarValidator();
            var resltvalid = validations.Validate(dto.Stars);
            if (resltvalid.IsValid)
            {
                var result = await _buyerPostStarService.CreateAsync(dto);

                return Ok(result);
            }
            else
                return BadRequest(resltvalid.Errors);
        }

        //[HttpPut("{postId}")]
        //public async Task<IActionResult> UpdateAsync([FromForm] long postId, [FromForm] BuyerPostStarUpdateDto dto)
        //{
        //    PostStarValidator validations = new PostStarValidator();
        //    var resltvalid = validations.Validate(dto.Stars);
        //    if (resltvalid.IsValid)
        //    {
        //        var result = await _buyerPostStarService.UpdateAsync(postId, dto);

        //        return Ok(result);
        //    }
        //    else
        //        return BadRequest(resltvalid.Errors);
        //}
    }
}
