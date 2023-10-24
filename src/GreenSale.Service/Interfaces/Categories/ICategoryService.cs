using GreenSale.Application.Utils;
using GreenSale.Domain.Entites.Categories;
using GreenSale.Persistence.Dtos.CategoryDtos;

namespace GreenSale.Service.Interfaces.Categories;

public interface ICategoryService
{
    public Task<bool> CreateAsync(CategoryCreateDto dto);
    public Task<bool> DeleteAsync(long categoryId);
    public Task<bool> UpdateAsync(long categoryID, CategoryCreateDto dto);
    public Task<long> CountAsync();
    public Task<List<Category>> GetAllAsync(PaginationParams @params);
    public Task<Category> GetBYIdAsync(long categoryId);
}
