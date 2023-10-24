using GreenSale.Application.Utils;

namespace GreenSale.DataAccess.Interfaces
{
    public interface IRepository<TEntity, TViewModel>
    {
        public Task<int> CreateAsync(TEntity entity);
        public Task<int> UpdateAsync(long Id, TEntity entity);
        public Task<long> CountAsync();
        public Task<List<TViewModel>> GetAllAsync(PaginationParams @params);
        public Task<TViewModel> GetByIdAsync(long Id);
        public Task<int> DeleteAsync(long Id);
    }
}