using GreenSale.Application.Utils;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.Domain.Entites.SelerPosts;
using GreenSale.Persistence.Dtos.SellerPostImageUpdateDtos;
using GreenSale.Persistence.Dtos.SellerPostsDtos;

namespace GreenSale.Service.Interfaces.SellerPosts;

public interface ISellerPostService
{
    public Task<bool> CreateAsync(SellerPostCreateDto dto);
    public Task<bool> DeleteAsync(long sellerId);
    public Task<bool> DeleteImageIdAsync(long ImageId);
    public Task<bool> UpdateAsync(long sellerID, SellerPostUpdateDto dto);
    public Task<bool> UpdateStatusAsync(long postId, SellerPostStatusUpdateDto dto);
    public Task<bool> ImageUpdateAsync(long postImageId, SellerPostImageUpdateDto dto);
    public Task<long> CountAsync();
    public Task<long> CountStatusAgreeAsync();
    public Task<long> CountStatusNewAsync();
    public Task<List<SellerPostViewModel>> GetAllAsync(PaginationParams @params);
    public Task<List<SellerPostViewModel>> GetAllByIdAsync(long userId, PaginationParams @params);
    public Task<List<SellerPost>> GetAllByIdAsync(long sellerId);
    public Task<SellerPostViewModel> GetBYIdAsync(long sellerId);
    public Task<(long IteamCount, List<SellerPostViewModel>)> SearchAsync(string search);
    public Task<List<PostCreatedAt>> SellerDaylilyCreatedAsync(int day);
    public Task<List<PostCreatedAt>> SellerMonthlyCreatedAsync(int month);
}
