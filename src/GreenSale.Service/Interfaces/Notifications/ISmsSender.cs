using GreenSale.Persistence.Dtos.Notifications;

namespace GreenSale.Service.Interfaces.Notifications;

public interface ISmsSender
{
    public Task<bool> SendAsync(SmsSenderDto message);
}
