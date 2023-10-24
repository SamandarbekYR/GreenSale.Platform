using GreenSale.Application.Utils;
using GreenSale.Domain.Entites.Storages;
using GreenSale.Persistence.Dtos.SellerPostsDtos;
using GreenSale.Persistence.Dtos.StoragDtos;

namespace GreenSale.Service.Interfaces.Storages;

public interface IStoragePostStarService
{
    public Task<bool> CreateAsync(StorageStarCreateDto dto);
    public Task<bool> UpdateAsync(long PostId, StorageStarUpdateDto dto);
    public Task<long> CountAsync();
    public Task<List<StoragePostStars>> GetAllAsync(PaginationParams @params);
    public Task<StoragePostStars> GetByIdAsync(long Id);
    public Task<bool> DeleteAsync(long userId, long postId);
    public Task<double> AvarageStarAsync(long postid);
    public Task<int> GetUserStarAsync(long postId);
}
