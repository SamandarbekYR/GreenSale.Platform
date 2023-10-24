namespace GreenSale.Persistence.Dtos.Auth;

public class ForgotPassword
{
    public string PhoneNumber { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
