using GreenSale.Domain.Entites.SellerPosts;

namespace GreenSale.DataAccess.Interfaces.SellerPosts
{
    public interface ISellerPostImageRepository : IRepository<SellerPostImage, SellerPostImage>
    {
        public Task<List<SellerPostImage>> GetFirstAllAsync();
        public Task<List<SellerPostImage>> GetByIdAllAsync(long Id);
    }
}