using GreenSale.Application.Utils;
using GreenSale.Domain.Entites.SellerPosts;
using GreenSale.Persistence.Dtos.SellerPostsDtos;

namespace GreenSale.Service.Interfaces.SellerPosts
{
    public interface ISellerPostStarService
    {
        public Task<bool> CreateAsync(SellerPostStarCreateDto dto);
        public Task<bool> UpdateAsync(long Id, SellerPostStarUpdateDto dto);
        public Task<long> CountAsync();
        public Task<List<SellerPostStars>> GetAllAsync(PaginationParams @params);
        public Task<SellerPostStars> GetByIdAsync(long Id);
        public Task<bool> DeleteAsync(long userId,long postId);
        public Task<double> AvarageStarAsync(long postid);
        public Task<int> GetUserStarAsync(long postId);
    }
}
