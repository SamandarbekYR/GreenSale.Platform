namespace GreenSale.Persistence.Dtos.Notifications;

public class SmsSenderDto
{
    public string Recipent { get; set; } = String.Empty;
    public string Title { get; set; } = String.Empty;
    public string Content { get; set; } = String.Empty;
}
