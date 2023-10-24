using GreenSale.Application.Utils;
using GreenSale.Service.Interfaces.BuyerPosts;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Common.BuyerPosts
{
    [Route("api/common/buyer/posts")]
    [ApiController]
    public class CommonBuyerPostsController : BaseController
    {
        private IBuyerPostService _service;
        private readonly int maxPage = 20;
        public CommonBuyerPostsController(IBuyerPostService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1)
            => Ok(await _service.GetAllAsync(new PaginationParams(page, maxPage)));

        [HttpGet("all/{userId}")]
        public async Task<IActionResult> GetAllByIdAsync(long userId, [FromQuery] int page = 1)
        => Ok(await _service.GetAllByIdAsync(userId, new PaginationParams(page, maxPage)));

        [HttpGet("{postId}")]
        public async Task<IActionResult> GetByIdAsync(long postId)
            => Ok(await _service.GetBYIdAsync(postId));

        [HttpGet("count")]
        public async Task<IActionResult> CountAsync()
            => Ok(await _service.CountAsync());

        [HttpGet("agreed/count")]
        public async Task<IActionResult> CountAgreeAsync()
            => Ok(await _service.CountStatusAgreeAsync());

        [HttpGet("new/count")]
        public async Task<IActionResult> CountNewAsync()
           => Ok(await _service.CountStatusNewAsync());

        [HttpGet("search/title")]
        public async Task<IActionResult> SearchingAsync(string search)
        {
            var res = await _service.SearchingAsync(search);

            return Ok(new { res.IteamCount, res.Item2 });
        }

        [HttpGet("created/daylily/count")]
        public async Task<IActionResult> GetCreatedDaylilyAsync([FromQuery] int day)
            => Ok(await _service.BuyerDaylilyCreatedAsync(day));

        [HttpGet("created/monthly/count")]
        public async Task<IActionResult> GetCreatedMonthlyAsync([FromQuery] int month)
            => Ok(await _service.BuyerMonthlyCreatedAsync(month));
    }
}
