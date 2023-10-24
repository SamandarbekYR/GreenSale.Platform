namespace GreenSale.Domain.Entites.BuyerPosts;

public class BuyerPostStars:Auditable
{
    public long UserId { get; set; }
    public long PostId { get; set; }
    public int Stars { get; set; }
}
