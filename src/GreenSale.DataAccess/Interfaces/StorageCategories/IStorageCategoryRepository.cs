using GreenSale.Domain.Entites.Storages;

namespace GreenSale.DataAccess.Interfaces.StorageCategories
{
    public interface IStorageCategoryRepository : IRepository<StorageCategory, StorageCategory>
    {
        public Task<long> GetCategoriesAsync(long storageId);
        public Task<List<long>> GetStorageIdAsync(long categoryId);
    }
}