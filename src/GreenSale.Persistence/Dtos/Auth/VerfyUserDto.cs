namespace GreenSale.Persistence.Dtos.Auth;

public class VerfyUserDto
{
    public string PhoneNumber { get; set; } = string.Empty;
    public int Code { get; set; }
}
