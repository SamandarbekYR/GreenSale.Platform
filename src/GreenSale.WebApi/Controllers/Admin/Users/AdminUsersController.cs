using GreenSale.Application.Utils;
using GreenSale.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenSale.WebApi.Controllers.Admin.Users
{
    [Route("api/admin/users")]
    [ApiController]
    public class AdminUsersController : AdminBaseController
    {
        private readonly IUserService _userService;
        private readonly int maxPage = 20;

        public AdminUsersController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] int page = 1)
        => Ok(await _userService.GetAllAsync(new PaginationParams(page, maxPage)));

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByIdAsync(int userId)
            => Ok(await _userService.GetByIdAsync(userId));

        [HttpGet("count")]
        public async Task<IActionResult> CountAsync()
            => Ok(await _userService.CountAsync());

        [HttpGet("admin")]
        public async Task<IActionResult> GetAllAdminAsync([FromQuery] int page = 1)
       => Ok(await _userService.GetAllAdminAsync(new PaginationParams(page, maxPage)));

        [HttpGet("only/users")]
        public async Task<IActionResult> GetAllUserAsync([FromQuery] int page = 1)
     => Ok(await _userService.GetAllUserAsync(new PaginationParams(page, maxPage)));

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteAsync(long userId)
            => Ok(await _userService.DeleteAsync(userId));

        [HttpGet("search")]
        public async Task<IActionResult> SearchAsync([FromQuery] string search)
        {

             var res = (await _userService.SearchAsync(search));
        
            return Ok(new { res.IteamCount, res.Item2 });
        }
    }
}
