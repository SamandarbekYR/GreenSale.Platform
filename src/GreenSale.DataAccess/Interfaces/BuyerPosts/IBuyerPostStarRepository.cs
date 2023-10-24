
using GreenSale.Domain.Entites.BuyerPosts;

namespace GreenSale.DataAccess.Interfaces.BuyerPosts;

public interface IBuyerPostStarRepository : IRepository<BuyerPostStars,BuyerPostStars>
{
    public Task<List<int>> GetAllStarsPostIdCountAsync(long postid);
    public Task<long> GetIdAsync(long userid, long postid);
    public Task<bool> DeleteUserAsync(long userId);
}
