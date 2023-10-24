using GreenSale.WebApi.Controllers.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Client
{
    [Authorize(Roles = "User")]
    public class BaseClientController : ControllerBase
    { }
}
