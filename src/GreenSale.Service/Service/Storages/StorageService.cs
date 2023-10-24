using GreenSale.Application.Exceptions;
using GreenSale.Application.Exceptions.Categories;
using GreenSale.Application.Exceptions.Storages;
using GreenSale.Application.Exceptions.Users;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Categories;
using GreenSale.DataAccess.Interfaces.StorageCategories;
using GreenSale.DataAccess.Interfaces.Storages;
using GreenSale.DataAccess.Interfaces.Users;
using GreenSale.DataAccess.ViewModels.BuyerPosts;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.DataAccess.ViewModels.Storages;
using GreenSale.Domain.Entites.Storages;
using GreenSale.Persistence.Dtos.StoragDtos;
using GreenSale.Service.Helpers;
using GreenSale.Service.Interfaces.Auth;

using GreenSale.Service.Interfaces.Common;
using GreenSale.Service.Interfaces.Storages;
using Microsoft.AspNetCore.Server.IIS.Core;

namespace GreenSale.Service.Service.Storages;

public class StorageService : IStoragesService
{
    private IUserRepository _userep;
    private IIdentityService _identity;
    private IStorageRepository _repository;
    private IStorageCategoryRepository _storagecategoryRepository;
    private IPaginator _paginator;
    private IFileService _fileService;
    private ICategoryRepository _categoryRepository;
    private IStoragePostStarService _storagePostStarService;
    private readonly string STORAGEPOSTIMAGES = "StoragePostImages";

    public StorageService(
        IStorageRepository repository,
        IPaginator paginator,
        IFileService fileService,
        IIdentityService identity,
        IUserRepository userRepository,
        IStorageCategoryRepository storagecategoryRepository,
        ICategoryRepository categoryRepository,
        IStoragePostStarService storagePostStarService)
    {
        this._userep = userRepository;
        this._identity = identity;
        this._repository = repository;
        this._paginator = paginator;
        this._fileService = fileService;
        this._storagecategoryRepository = storagecategoryRepository;
        this._categoryRepository = categoryRepository;
        this._storagePostStarService = storagePostStarService;
    }

    public async Task<long> CountAsync()
    {
        return await _repository.CountAsync();
    }

    public async Task<bool> CreateAsync(StoragCreateDto dto)
    {
        var resultcategory = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (resultcategory is null)
        {
            throw new CategoryNotFoundException();
        }
        else
        {
            string imagePath = await _fileService.UploadImageAsync(dto.ImagePath, STORAGEPOSTIMAGES);
            Storage storage = new Storage()
            {
                UserId = _identity.Id,
                Name = dto.Name,
                Description = dto.Description,
                Region = dto.Region,
                District = dto.District,
                Address = dto.Address,
                Info = dto.Info,
                AddressLatitude = dto.AddressLatitude,
                AddressLongitude = dto.AddressLongitude,
                CreatedAt = TimeHelper.GetDateTime(),
                UpdatedAt = TimeHelper.GetDateTime(),
                ImagePath = imagePath
            };

            StorageCategory storagecategory = new StorageCategory();
            storagecategory.CategoryId = dto.CategoryId;
            storagecategory.UserId = _identity.Id;
            storagecategory.CreatedAt = storagecategory.UpdatedAt = TimeHelper.GetDateTime();

            var result2 = await _repository.CreateAsync(storage);

            storagecategory.StorageId = result2;

            var result1 = await _storagecategoryRepository.CreateAsync(storagecategory);

            return result1 > 0 && result2 > 0;
        }
    }

    public async Task<bool> DeleteAsync(long storageId)
    {
        var storageGet = await _repository.GetByIdAsync(storageId);

        if (storageGet.Id == 0)
            throw new StorageNotFoundException();

        var delstoragecategoryresult = await _storagecategoryRepository.DeleteAsync(storageGet.Id);

        var deleteImage = await _fileService.DeleteImageAsync(storageGet.ImagePath);

        /*if (deleteImage == false)
            throw new ImageNotFoundException();*/

        var deletestarresult = await _storagePostStarService.DeleteAsync(storageGet.UserId,  storageId);
        var result = await _repository.DeleteAsync(storageId);

        return result > 0 && deletestarresult;
    }

    public async Task<List<StoragesViewModel>> GetAllAsync(PaginationParams @params)
    {
        var getAll = await _repository.GetAllAsync(@params);

        foreach (var item in getAll)
        {
            item.AverageStars = await _storagePostStarService.AvarageStarAsync(item.Id);
            item.UserStars = await _storagePostStarService.GetUserStarAsync(item.Id);
            long categoryId = await _storagecategoryRepository.GetCategoriesAsync(item.Id);
            item.StorageCategory = await _categoryRepository.GetCategoryNameAsync(categoryId);
        }

        var count = await _repository.CountAsync();
        _paginator.Paginate(count, @params);

        return getAll;
    }

    public async Task<StoragesViewModel> GetBYIdAsync(long storageId)
    {
        var getId = await _repository.GetByIdAsync(storageId);

        getId.AverageStars = await _storagePostStarService.AvarageStarAsync(getId.Id);
        getId.UserStars = await _storagePostStarService.GetUserStarAsync(getId.Id);
        long categoryId = await _storagecategoryRepository.GetCategoriesAsync(getId.Id);
        getId.StorageCategory = await _categoryRepository.GetCategoryNameAsync(categoryId);

        if (getId.Id == 0)
            throw new StorageNotFoundException();

        return getId;
    }

    public async Task<bool> UpdateAsync(long storageID, StoragUpdateDto dto)
    {
        var getId = await _repository.GetByIdAsync(storageID);

        if (getId.Id == 0)
            throw new StorageNotFoundException();

        Storage storage = new Storage()
        {
            Name = dto.Name,
            UserId = getId.UserId,
            Address = dto.Address,
            Info = dto.Info,
            Description = dto.Description,
            District = dto.District,
            Region = dto.Region,
            AddressLongitude = dto.AddressLongitude,
            AddressLatitude = dto.AddressLatitude,
            CreatedAt = getId.CreatedAt,
            UpdatedAt = TimeHelper.GetDateTime()
        };

        //if (dto.ImagePath is not null)
        //{
        //    //delete old image
        //    var deleteImage = await _fileService.DeleteImageAsync(getId.ImagePath);


        //    //upload new image
        //    string imagePath = await _fileService.UploadImageAsync(dto.ImagePath, STORAGEPOSTIMAGES);
        //    storage.ImagePath = imagePath;
        //}

        var result = await _repository.UpdateAsync(storageID, storage);

        return result > 0;
    }

    public async Task<bool> UpdateImageAsync(long storageID, StorageImageUpdateDto dto)
    {
        var DbFound = await _repository.GetByIdAsync(storageID);

        if (DbFound.Id == 0) throw new StorageNotFoundException();

        var img = await _fileService.DeleteImageAsync(DbFound.ImagePath);
        var res = await _fileService.UploadImageAsync(dto.StorageImage, STORAGEPOSTIMAGES);
        DbFound.ImagePath = res;

        //Storage storage = new Storage()
        //{
        //    Name = DbFound.FullName.Split(' ')[0],
        //    UserId = DbFound.UserId,
        //    Description = DbFound.Description,
        //    Region = DbFound.Region,
        //    District = DbFound.District,
        //    Address = DbFound.Address,
        //    AddressLatitude = DbFound.AddressLatitude,
        //    AddressLongitude = DbFound.AddressLongitude,
        //    Info = DbFound.Info,
        //    CreatedAt = DbFound.CreatedAt,
        //    UpdatedAt = TimeHelper.GetDateTime(),
        //    ImagePath = res
        //};
        var Result = await _repository.UpdateImageAsync(storageID,res);

        return Result > 0;
    }

    public async Task<List<StoragesViewModel>> GetAllByIdAsync(long userId, PaginationParams @params)
    {
        var userdev = await _userep.GetByIdAsync(userId);
        if (userdev.Id == 0)
            throw new UserNotFoundException();

        var DbFound = await _repository.GetAllByIdAsync(userId, @params);

        foreach (var item in DbFound)
        {
            item.AverageStars = await _storagePostStarService.AvarageStarAsync(item.Id);
            item.UserStars = await _storagePostStarService.GetUserStarAsync(item.Id);
            long categoryId = await _storagecategoryRepository.GetCategoriesAsync(item.Id);
            item.StorageCategory = await _categoryRepository.GetCategoryNameAsync(categoryId);
        }

        var count = await _repository.CountAsync();
        _paginator.Paginate(count, @params);

        return DbFound;
    }

    public async Task<(long IteamCount, List<StoragesViewModel>)> SearchAsync(string search)
    {
        var res = await _repository.SearchAsync(search);

        if(res.ItemsCount == 0)
        {
            List<StoragesViewModel> empty = new List<StoragesViewModel>();
            return (0, empty);
        }

        foreach (var item in res.Item2)
        {
            item.AverageStars = await _storagePostStarService.AvarageStarAsync(item.Id);
            item.UserStars = await _storagePostStarService.GetUserStarAsync(item.Id);
            long categoryId = await _storagecategoryRepository.GetCategoriesAsync(item.Id);
            item.StorageCategory = await _categoryRepository.GetCategoryNameAsync(categoryId);
        }

        return res;
    }

    public async Task<List<PostCreatedAt>> StorageDaylilyCreatedAsync(int day)
    {
        var result = await _repository.StorageDaylilyCreatedAsync(day.ToString());

        return result;
    }

    public async Task<List<PostCreatedAt>> StorageMonthlyCreatedAsync(int month)
    {
        var result = await _repository.StorageMonthlyCreatedAsync(month.ToString());

        return result;
    }
}
