using GreenSale.Application.Utils;
using GreenSale.DataAccess.Common;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.Domain.Entites.SelerPosts;

namespace GreenSale.DataAccess.Interfaces.SellerPosts
{
    public interface ISellerPostsRepository : IRepository<SellerPost, SellerPostViewModel>, ISearchable<SellerPostViewModel>
    {
        public Task<SellerPost> GetIdAsync(long postId);
        public Task<List<SellerPostViewModel>> GetAllByIdAsync(long userId, PaginationParams @params);
        public Task<List<SellerPostViewModel>> GetAllByIdAsync(long userId);
        public Task<List<SellerPost>> GetAllByIdSellerAsync(long categotyId);
        public Task<long> CountStatusAgreeAsync();
        public Task<long> CountStatusNewAsync();
        public Task<List<PostCreatedAt>> SellerDaylilyCreatedAsync(string day);
        public Task<List<PostCreatedAt>> SellerMonthlyCreatedAsync(string month);
    }
}