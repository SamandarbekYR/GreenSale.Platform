using GreenSale.Persistence.Dtos.BuyerPostsDto;
using GreenSale.Persistence.Validators.PostStar;
using GreenSale.Service.Interfaces.BuyerPosts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Client.BuyerPosts
{
    [Route("api/client/buyer/star")]
    [ApiController]
    public class ClientBuyerPostStarController : BaseClientController
    {
        private readonly IBuyerPostStarService _buyerPostStarService;
        public ClientBuyerPostStarController(IBuyerPostStarService buyerPostStarService)
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
