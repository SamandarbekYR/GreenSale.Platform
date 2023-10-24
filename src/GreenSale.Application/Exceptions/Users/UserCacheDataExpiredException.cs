namespace GreenSale.Application.Exceptions.Users;

public class UserCacheDataExpiredException : ExpiredException
{
    public UserCacheDataExpiredException()
    {
        this.TitleMessage = "User data has expired!";
    }
}