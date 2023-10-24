using GreenSale.Domain.Entites.BuyerPosts;

namespace GreenSale.DataAccess.Interfaces.BuyerPosts
{
    public interface IBuyerPostImageRepository : IRepository<BuyerPostImage, BuyerPostImage>
    {
        public Task<List<BuyerPostImage>> GetByIdAllAsync(long Id);
        public Task<List<BuyerPostImage>> GetFirstAllAsync();
    }
}
