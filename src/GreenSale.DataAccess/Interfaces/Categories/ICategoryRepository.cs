using GreenSale.Domain.Entites.Categories;

namespace GreenSale.DataAccess.Interfaces.Categories
{
    public interface ICategoryRepository : IRepository<Category, Category>
    {
        public Task<string> GetCategoryNameAsync(long categoryId);
    }
}