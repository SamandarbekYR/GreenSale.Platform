namespace GreenSale.Domain.Entites.SellerPosts;

public class SellerPostStars:Auditable
{
    public long UserId { get; set; }
    public long PostId { get; set; }
    public int Stars { get; set; }
}
