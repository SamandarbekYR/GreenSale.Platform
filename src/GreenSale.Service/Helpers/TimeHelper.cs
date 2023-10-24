using GreenSale.Domain.Constants;

namespace GreenSale.Service.Helpers;

public class TimeHelper
{
    public static DateTime GetDateTime()
    {
        var time = DateTime.UtcNow;
        time = time.AddHours(TimeConstant.UTC);

        return time;
    }
}
