using GreenSale.DataAccess.ViewModels.UserRoles;

namespace GreenSale.Service.Interfaces.Auth;

public interface ITokenService
{
    public string GenerateToken(UserRoleViewModel user);
}
