using GreenSale.Application.Utils;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.DataAccess.ViewModels.Storages;
using GreenSale.Persistence.Dtos.StoragDtos;

namespace GreenSale.Service.Interfaces.Storages
{
    public interface IStoragesService
    {
        public Task<bool> CreateAsync(StoragCreateDto dto);
        public Task<bool> DeleteAsync(long storageId);
        public Task<bool> UpdateAsync(long storageID, StoragUpdateDto dto);
        public Task<bool> UpdateImageAsync(long storageID, StorageImageUpdateDto dto);
        public Task<long> CountAsync();
        public Task<List<StoragesViewModel>> GetAllAsync(PaginationParams @params);
        public Task<List<StoragesViewModel>> GetAllByIdAsync(long userId, PaginationParams @params);
        public Task<StoragesViewModel> GetBYIdAsync(long storageId);
        public Task<(long IteamCount, List<StoragesViewModel>)> SearchAsync(string search);
        public Task<List<PostCreatedAt>> StorageDaylilyCreatedAsync(int day);
        public Task<List<PostCreatedAt>> StorageMonthlyCreatedAsync(int month);
    }
}
