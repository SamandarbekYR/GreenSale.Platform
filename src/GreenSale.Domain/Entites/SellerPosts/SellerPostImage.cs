namespace GreenSale.Domain.Entites.SellerPosts
{
    public class SellerPostImage : Auditable
    {
        public long SellerPostId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
