using GreenSale.Persistence.Dtos.SellerPostsDtos;
using GreenSale.Persistence.Validators.PostStar;
using GreenSale.Service.Interfaces.SellerPosts;
using GreenSale.WebApi.Controllers.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Admin.SellerPosts
{
    [Route("api/admin/seller/post/star")]
    [ApiController]
    public class AdminSellerPostStarController : AdminBaseController
    {
        private readonly ISellerPostStarService _sellerPostStarService;
        public AdminSellerPostStarController(ISellerPostStarService sellerPostStarService)
        {
            _sellerPostStarService = sellerPostStarService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm] SellerPostStarCreateDto dto)
        {
            PostStarValidator validations = new PostStarValidator();
            var resltvalid = validations.Validate(dto.Stars);
            if (resltvalid.IsValid)
            {
                var result = await _sellerPostStarService.CreateAsync(dto);

                return Ok(result);
            }
            else
                return BadRequest(resltvalid.Errors);
        }

        //[HttpPut("{postId}")]
        //public async Task<IActionResult> UpdateAsync([FromForm] long postId, [FromForm] SellerPostStarUpdateDto dto)
        //{
        //    PostStarValidator validations = new PostStarValidator();
        //    var resltvalid = validations.Validate(dto.Stars);
         //   if (resltvalid.IsValid)
         //   {
         //       var result = await _sellerPostStarService.UpdateAsync(postId, dto);
         //
           //     return Ok(result);
          //  }
          //  else
            //    return BadRequest(resltvalid.Errors);
        //}
    }
}
