using GreenSale.Application.Exceptions.Categories;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Categories;
using GreenSale.DataAccess.Interfaces.StorageCategories;
using GreenSale.Domain.Entites.Categories;
using GreenSale.Persistence.Dtos.CategoryDtos;
using GreenSale.Service.Helpers;
using GreenSale.Service.Interfaces.BuyerPosts;
using GreenSale.Service.Interfaces.Categories;
using GreenSale.Service.Interfaces.Common;
using GreenSale.Service.Interfaces.SellerPosts;
using GreenSale.Service.Interfaces.Storages;

namespace GreenSale.Service.Service.Categories
{
    public class CategoryService : ICategoryService
    {
        private ISellerPostService _sellerservice;
        private IBuyerPostService _buyerservice;
        private IStoragesService _storageservice;
        private IStorageCategoryRepository _storageCategoryRepository;
        private ICategoryRepository _repository;
        private IPaginator _paginator;

        public CategoryService(
            ICategoryRepository repository,
            IPaginator paginator,
            ISellerPostService sellerPostService,
            IStorageCategoryRepository storageCategoryRepository,
            IBuyerPostService buyerPostService,
            IStoragesService storagesService)
        {
            this._sellerservice = sellerPostService;
            this._buyerservice = buyerPostService;
            this._storageCategoryRepository = storageCategoryRepository;
            this._storageservice = storagesService;
            this._repository = repository;
            _paginator = paginator;
        }

        public async Task<long> CountAsync()
        {
            return await _repository.CountAsync();
        }

        public async Task<bool> CreateAsync(CategoryCreateDto dto)
        {
            Category category = new Category()
            {
                Name = dto.Name,
                CreatedAt = TimeHelper.GetDateTime(),
                UpdatedAt = TimeHelper.GetDateTime()
            };

            var result = await _repository.CreateAsync(category);

            return result > 0;
        }

        public async Task<bool> DeleteAsync(long categoryId)
        {
            var category = await _repository.GetByIdAsync(categoryId);

            if (category.Id == 0)
            {
                throw new CategoryNotFoundException();
            }

            var seller = await _sellerservice.GetAllByIdAsync(categoryId);
            var buyer = await _buyerservice.GetAllByIdAsync(categoryId);
            foreach (var item in seller)
            {
                await _sellerservice.DeleteAsync(item.Id);
            }

            foreach (var item in buyer)
            {
                await _buyerservice.DeleteAsync(item.Id);
            }

            List<long> storageId = await _storageCategoryRepository.GetStorageIdAsync(categoryId);
            var storagecategory = await _storageCategoryRepository.DeleteAsync(categoryId);
            var result = await _repository.DeleteAsync(categoryId);

            foreach (var id in storageId)
            {
                await  _storageservice.DeleteAsync(id);
            }

            return result > 0;
        }

        public async Task<List<Category>> GetAllAsync(PaginationParams @params)
        {
            var categories = await _repository.GetAllAsync(@params);
            var count = await _repository.CountAsync();
            _paginator.Paginate(count, @params);

            return categories;
        }

        public async Task<Category> GetBYIdAsync(long categoryId)
        {
            var categories = await _repository.GetByIdAsync(categoryId);

            if (categories.Id == 0)
                throw new CategoryNotFoundException();

            return categories;
        }

        public async Task<bool> UpdateAsync(long categoryID, CategoryCreateDto dto)
        {
            var categories = await _repository.GetByIdAsync(categoryID);

            if (categories.Id == 0)
                throw new CategoryNotFoundException();

            categories.Name = dto.Name;
            categories.UpdatedAt = TimeHelper.GetDateTime();
            var result = await _repository.UpdateAsync(categoryID, categories);

            return result > 0;
        }
    }
}
