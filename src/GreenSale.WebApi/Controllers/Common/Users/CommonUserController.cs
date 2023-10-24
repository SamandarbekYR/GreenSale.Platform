using GreenSale.Application.Utils;
using GreenSale.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GreenSale.WebApi.Controllers.Common.Users
{
    [Route("api/common/users")]
    [ApiController]
    public class CommonUserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly int maxPage = 20;

        public CommonUserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAsync()
            => Ok(await _userService.CountAsync());
    }
}
