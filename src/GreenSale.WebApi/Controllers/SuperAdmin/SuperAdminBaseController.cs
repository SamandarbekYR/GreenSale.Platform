using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.SuperAdmin;

[Authorize(Roles = "SuperAdmin")]
public class SuperAdminBaseController : ControllerBase
{ }
