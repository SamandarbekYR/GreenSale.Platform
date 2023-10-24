namespace GreenSale.Application.Exceptions.Roles;

public class RoleNotFoundException : NotFoundException
{
    public RoleNotFoundException()
    {
        TitleMessage = "Role not found!";
    }
}
