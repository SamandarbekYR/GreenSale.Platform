namespace GreenSale.Application.Exceptions.Auth;

public class VerificationCodeExpiredException : ExpiredException
{
    public VerificationCodeExpiredException()
    {
        this.TitleMessage = "Verification code is expired!";
    }
}
