using GreenSale.Application.Utils;
using GreenSale.DataAccess.Common;
using GreenSale.DataAccess.ViewModels.BuyerPosts;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.Domain.Entites.BuyerPosts;

namespace GreenSale.DataAccess.Interfaces.BuyerPosts;

public interface IBuyerPostRepository : IRepository<BuyerPost, BuyerPostViewModel>, ISearchable<BuyerPostViewModel>
{
    public Task<BuyerPost> GetIdAsync(long buyerId);
    public Task<List<BuyerPostViewModel>> GetAllByIdAsync(long userId, PaginationParams @params);
    public Task<List<BuyerPostViewModel>> GetAllByIdAsync(long userId);
    public Task<List<BuyerPostViewModel>> GetAllByIdBuyerAsync(long buyerId);
    public Task<long> CountStatusAgreeAsync();
    public Task<long> CountStatusNewAsync();
    public Task<List<PostCreatedAt>> BuyerDaylilyCreatedAsync(string day);
    public Task<List<PostCreatedAt>> BuyerMonthlyCreatedAsync(string month);

}