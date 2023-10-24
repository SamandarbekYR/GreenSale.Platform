using GreenSale.Persistence.Dtos;
using GreenSale.Persistence.Dtos.Auth;

namespace GreenSale.Service.Interfaces.Auth;

public interface IAuthServices
{
    public Task<(bool Result, int CachedMinutes)> RegisterAsync(UserRegisterDto dto);
    public Task<(bool Result, int CachedVerificationMinutes)> SendCodeForRegisterAsync(string phoneNumber);
    public Task<(bool Result, string Token)> VerifyRegisterAsync(string phoneNumber, int code);
    public Task<(bool Result, string Token)> LoginAsync(UserLoginDto dto);
    public Task<(bool Result, int CachedMinutes)> ResetPasswordAsync(ForgotPassword dto);
    public Task<(bool Result, string Token)> VerifyResetPasswordAsync(string phoneNumber, int code);
    public Task<bool> CheckTokenAsync(AuthorizationDto token);
}
