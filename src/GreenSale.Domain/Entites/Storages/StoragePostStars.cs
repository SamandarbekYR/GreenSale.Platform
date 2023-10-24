namespace GreenSale.Domain.Entites.Storages;

public class StoragePostStars:Auditable
{
    public long UserId { get; set; }
    public long PostId { get; set; }
    public int Stars { get; set; }
}
