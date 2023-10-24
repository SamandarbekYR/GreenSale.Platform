namespace GreenSale.Application.Exceptions.Auth;

public class VerificationTooManyRequestsException : TooManyRequestException
{
    public VerificationTooManyRequestsException()
    {
        this.TitleMessage = "You tried more than limits!";
    }
}