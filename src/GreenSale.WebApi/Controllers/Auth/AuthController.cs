using GreenSale.Persistence.Dtos;
using GreenSale.Persistence.Dtos.Auth;
using GreenSale.Persistence.Validators;
using GreenSale.Service.Interfaces.Auth;
using GreenSale.Service.Interfaces.Roles;
using GreenSale.WebApi.Controllers.Common;
using GreenSaleuz.Persistence.Validators.Dtos.AuthUserValidators;
using Microsoft.AspNetCore.Mvc;

namespace GreenSale.WebApi.Controllers.Auth;

[Route("api/auth")]
[ApiController]
public class AuthController : BaseController
{
    private readonly IAuthServices _authService;
    private readonly IUserRoleService _userRole;

    public AuthController(IUserRoleService userRole,
        IAuthServices authServices,
        IConfiguration configuration)
    {
        _authService = authServices;
        _userRole = userRole;
    }
    [HttpGet("check/user/role")]
    public async Task<IActionResult> GetUserRole()
        => Ok (await _userRole.GetRole());

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromForm] UserRegisterDto dto)
    {
        UserRegisterValidator validations = new UserRegisterValidator();
        var resltvalid = validations.Validate(dto);
        if (resltvalid.IsValid)
        {
            var result = await _authService.RegisterAsync(dto);

            return Ok(new { result.Result, result.CachedMinutes });
        }
        else
            return BadRequest(resltvalid.Errors);

    }

    [HttpPost("register/send-code")]
    public async Task<IActionResult> SendCodeAsync(string phone)
    {
        var valid = PhoneNumberValidator.IsValid(phone);
        if (valid)
        {
            var result = await _authService.SendCodeForRegisterAsync(phone);

            return Ok(new { result.Result, result.CachedVerificationMinutes });
        }
        else
            return BadRequest("Phone number invalid");

    }

    [HttpPost("register/verify")]
    public async Task<IActionResult> VerifyRegisterAsync([FromBody] VerfyUserDto dto)
    {
        var res = PhoneNumberValidator.IsValid(dto.PhoneNumber);
        if (res == false) return BadRequest("Phone number is invalid!");
        var srResult = await _authService.VerifyRegisterAsync(dto.PhoneNumber, dto.Code);
        return Ok(new { srResult.Result, srResult.Token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] UserLoginDto dto)
    {
        var res = PhoneNumberValidator.IsValid(dto.PhoneNumber);
        if (res == false)
            return BadRequest("Phone number is invalid!");

        var serviceResult = await _authService.LoginAsync(dto);

        return Ok(new { serviceResult.Result, serviceResult.Token });
    }

    [HttpPost("password/reset")]
    public async Task<IActionResult> ResetPassword([FromBody] ForgotPassword forgot)
    {
        var res = PhoneNumberValidator.IsValid(forgot.PhoneNumber);
        var password = PasswordValidator.IsStrongPassword(forgot.NewPassword);
        if (res == false)
            return BadRequest("Phone number is invalid!");
        else if (password.IsValid == false)
            return BadRequest(password.Message);

        var serviceResult = await _authService.ResetPasswordAsync(forgot);

        return Ok(new { serviceResult.Result, serviceResult.CachedMinutes });
    }

    [HttpPost("password/verify")]
    public async Task<IActionResult> PasswordVerifyAsync([FromBody] VerfyUserDto verfyUser)
    {
        var res = PhoneNumberValidator.IsValid(verfyUser.PhoneNumber);
        if (res == false) return BadRequest("Phone number is invalid!");
        var srResult = await _authService.VerifyResetPasswordAsync(verfyUser.PhoneNumber, verfyUser.Code);

        return Ok(new { srResult.Result, srResult.Token });
    }

    [HttpPost("token/verify")]
    public async Task<IActionResult> CheckToken([FromBody] AuthorizationDto token)
    {
        var requedt = await _authService.CheckTokenAsync(token);

        return Ok(requedt);
    }
}





