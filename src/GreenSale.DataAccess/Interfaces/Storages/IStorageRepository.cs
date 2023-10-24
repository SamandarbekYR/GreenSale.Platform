using GreenSale.Application.Utils;
using GreenSale.DataAccess.Common;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.DataAccess.ViewModels.Storages;
using GreenSale.Domain.Entites.Storages;
using static Dapper.SqlMapper;

namespace GreenSale.DataAccess.Interfaces.Storages
{
    public interface IStorageRepository : IRepository<Storage, StoragesViewModel>, ISearchable<StoragesViewModel>
    {
        public Task<List<StoragesViewModel>> GetAllByIdAsync(long userId, PaginationParams @params);
        public Task<List<StoragesViewModel>> GetAllByIdAsync(long userId);
        public Task<List<PostCreatedAt>> StorageDaylilyCreatedAsync(string day);
        public Task<List<PostCreatedAt>> StorageMonthlyCreatedAsync(string month);
        public Task<int> UpdateImageAsync(long Id, string imagePath);
    }
}