namespace GreenSale.Domain.Entites.BuyerPosts
{
    public class BuyerPostImage : Auditable
    {
        public long BuyerpostId { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }
}
