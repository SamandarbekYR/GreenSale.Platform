namespace GreenSale.Application.Exceptions.Roles;

public class UserRoleNotFoundException : NotFoundException
{
    public UserRoleNotFoundException()
    {
        TitleMessage = "User Role not found!";
    }
}
