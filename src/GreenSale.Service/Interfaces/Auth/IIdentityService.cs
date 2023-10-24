namespace GreenSale.Service.Interfaces.Auth;

public interface IIdentityService
{
    public long Id { get; }
    public string RoleName { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string PhoneNumber { get; }
}
