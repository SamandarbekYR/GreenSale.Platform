using GreenSale.Domain.Entites.SellerPosts;

namespace GreenSale.DataAccess.Interfaces.SellerPosts;

public interface ISellerPostStarRepository: IRepository<SellerPostStars, SellerPostStars>
{
    public Task<List<int>> GetAllStarsPostIdCountAsync(long postid);
    public Task<long> GetIdAsync(long userid, long postid);
    public Task<bool> DeleteUserAsync(long userId);
}
