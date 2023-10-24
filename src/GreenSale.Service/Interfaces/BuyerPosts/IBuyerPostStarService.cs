using GreenSale.Application.Utils;
using GreenSale.Domain.Entites.BuyerPosts;
using GreenSale.Persistence.Dtos.BuyerPostsDto;

namespace GreenSale.Service.Interfaces.BuyerPosts
{

    public interface IBuyerPostStarService
    {
        public Task<bool> CreateAsync(BuyerPostStarCreateDto dto);
        public Task<bool> UpdateAsync(long Id, BuyerPostStarUpdateDto dto);
        public Task<long> CountAsync();
        public Task<List<BuyerPostStars>> GetAllAsync(PaginationParams @params);
        public Task<BuyerPostStars> GetByIdAsync(long Id);
        public Task<bool> DeleteAsync(long userId, long postId);
        public Task<bool> DeleteUserAsync(long userId);
        public Task<double> AvarageStarAsync(long postid);
        public Task<int> GetUserStarAsync(long postId);
    }
}
