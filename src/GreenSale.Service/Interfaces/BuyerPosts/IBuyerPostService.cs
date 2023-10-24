using GreenSale.Application.Utils;
using GreenSale.DataAccess.ViewModels.BuyerPosts;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.Persistence.Dtos.BuyerPostImageUpdateDtos;
using GreenSale.Persistence.Dtos.BuyerPostsDto;

namespace GreenSale.Service.Interfaces.BuyerPosts;

public interface IBuyerPostService
{
    public Task<bool> CreateAsync(BuyerPostCreateDto dto);
    public Task<bool> DeleteAsync(long buyerId);
    public Task<bool> DeleteImageIdAsync(long ImageId);
    public Task<bool> UpdateAsync(long buyerID, BuyerPostUpdateDto dto);
    public Task<bool> UpdateStatusAsync(long buyerID, BuyerPostStatusUpdateDto dto);
    public Task<bool> ImageUpdateAsync(long ImageId, BuyerPostImageDto dto);
    public Task<long> CountAsync();
    public Task<long> CountStatusAgreeAsync();
    public Task<long> CountStatusNewAsync();
    public Task<List<BuyerPostViewModel>> GetAllAsync(PaginationParams @params);
    public Task<List<BuyerPostViewModel>> GetAllByIdAsync(long userId, PaginationParams @params);
    public Task<List<BuyerPostViewModel>> GetAllByIdAsync(long BuyerId);
    public Task<BuyerPostViewModel> GetBYIdAsync(long buyerId);
    public Task<(long IteamCount, List<BuyerPostViewModel>)> SearchingAsync(string search);
    public Task<List<PostCreatedAt>> BuyerDaylilyCreatedAsync(int day);
    public Task<List<PostCreatedAt>> BuyerMonthlyCreatedAsync(int month);
}
