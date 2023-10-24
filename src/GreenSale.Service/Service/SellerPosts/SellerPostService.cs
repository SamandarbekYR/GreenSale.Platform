using GreenSale.Application.Exceptions;
using GreenSale.Application.Exceptions.Categories;
using GreenSale.Application.Exceptions.SellerPosts;
using GreenSale.Application.Exceptions.Users;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Categories;
using GreenSale.DataAccess.Interfaces.SellerPosts;
using GreenSale.DataAccess.Interfaces.Users;
using GreenSale.DataAccess.ViewModels.BuyerPosts;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.Domain.Entites.SelerPosts;
using GreenSale.Domain.Entites.SellerPosts;
using GreenSale.Persistence.Dtos.SellerPostImageUpdateDtos;
using GreenSale.Persistence.Dtos.SellerPostsDtos;
using GreenSale.Service.Helpers;
using GreenSale.Service.Interfaces.Auth;
using GreenSale.Service.Interfaces.Common;
using GreenSale.Service.Interfaces.SellerPosts;

namespace GreenSale.Service.Service.SellerPosts;

public class SellerPostService : ISellerPostService
{
    private readonly IUserRepository _userep;
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISellerPostImageRepository _imageRepository;
    private readonly IFileService _fileservice;
    private readonly IPaginator _paginator;
    private readonly IIdentityService _identity;
    private readonly ISellerPostsRepository _repository;
    private readonly ISellerPostStarService _sellerPostStarService;
    private readonly string SELLERPOSTIMAGES = "SellerPostImages";

    public SellerPostService(
        ISellerPostsRepository repository,
        IIdentityService identity,
        IPaginator paginator,
        IFileService fileService,
        ISellerPostImageRepository imageRepository,
        ICategoryRepository categoryRepository,
        ISellerPostStarService sellerPostStarService,
        IUserRepository userRepository)
    {
        this._userep = userRepository;
        this._categoryRepository = categoryRepository;
        this._imageRepository = imageRepository;
        this._fileservice = fileService;
        this._paginator = paginator;
        this._sellerPostStarService= sellerPostStarService;
        this._identity = identity;
        this._repository = repository;
    }
    public async Task<long> CountAsync()
    {
        var DbResult = await _repository.CountAsync();

        return DbResult;
    }

    public async Task<long> CountStatusAgreeAsync()
    {
        var DbResult = await _repository.CountStatusAgreeAsync();

        return DbResult;
    }

    public async Task<long> CountStatusNewAsync()
    {
        var DbResult = await _repository.CountStatusNewAsync();

        return DbResult;
    }

    public async Task<bool> CreateAsync(SellerPostCreateDto dto)
    {
        var check = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (check.Id == 0)
        {
            throw new CategoryNotFoundException();
        }

        SellerPost sellerPost = new SellerPost()
        {
            UserId = _identity.Id,
            CategoryId = dto.CategoryId,
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Capacity = dto.Capacity,
            CapacityMeasure = dto.CapacityMeasure,
            Type = dto.Type,
            Region = dto.Region,
            District = dto.District,
            PhoneNumber = dto.PhoneNumber,
            Status = Domain.Enums.SellerPosts.SellerPostEnum.Nosold,
            CreatedAt = TimeHelper.GetDateTime(),
            UpdatedAt = TimeHelper.GetDateTime(),
        };

        var DbResult = await _repository.CreateAsync(sellerPost);

        if (DbResult > 0)
        {
            foreach (var item in dto.ImagePath)
            {
                var img = await _fileservice.UploadImageAsync(item, SELLERPOSTIMAGES);

                SellerPostImage sellerPostImage = new SellerPostImage()
                {
                    SellerPostId = DbResult,
                    ImagePath = img,
                    CreatedAt = TimeHelper.GetDateTime(),
                    UpdatedAt = TimeHelper.GetDateTime(),
                };

                var DbImgResult = await _imageRepository.CreateAsync(sellerPostImage);
            }

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(long sellerId)
    {
        var DbFound = await _repository.GetByIdAsync(sellerId);

        if (DbFound.Id == 0)
            throw new SellerPostsNotFoundException();

        var DbImgAll = await _imageRepository.GetByIdAllAsync(sellerId);

        /*if (DbImgAll.Count == 0)
            throw new ImageNotFoundException();*/

        var DbImgResult = await _imageRepository.DeleteAsync(sellerId);
        var delstarresult = await _sellerPostStarService.DeleteAsync(DbFound.UserId, sellerId);
        var Dbresult = await _repository.DeleteAsync(sellerId);

        if (DbImgResult > 0 && delstarresult  && Dbresult > 0)
        {
            if(DbImgAll.Count != 0)
            {
                foreach (var item in DbImgAll)
                {
                    await _fileservice.DeleteImageAsync(item.ImagePath);
                }
            }
        }

        return Dbresult > 0;
    }

    public async Task<bool> DeleteImageIdAsync(long ImageId)
    {
        var DbFound = await _imageRepository.GetByIdAsync(ImageId);

        if (DbFound.Id == 0)
            throw new ImageNotFoundException();

        await _fileservice.DeleteImageAsync(DbFound.ImagePath);
        var Dbresult = await _imageRepository.DeleteAsync(ImageId);

        return Dbresult > 0;
    }

    public async Task<List<SellerPostViewModel>> GetAllAsync(PaginationParams @params)
    {
        var DbResult = await _repository.GetAllAsync(@params);
        var dBim = await _imageRepository.GetFirstAllAsync();

        List<SellerPostViewModel> Result = new List<SellerPostViewModel>();

        foreach (var item in DbResult)
        {
            item.PostImages = new List<SellerPostImage>();
            item.AverageStars = await _sellerPostStarService.AvarageStarAsync(item.Id);
            item.UserStars=await _sellerPostStarService.GetUserStarAsync(item.Id);

            foreach (var img in dBim)
            {
                if (img.SellerPostId == item.Id)
                {
                    item.PostImages.Add(img);
                    item.MainImage = img.ImagePath;
                    dBim.RemoveAt(0);
                    break;
                }
            }

            Result.Add(item);
        }

        var DBCount = await _repository.CountAsync();
        _paginator.Paginate(DBCount, @params);

        return Result;
    }

    public async Task<List<SellerPostViewModel>> GetAllByIdAsync(long userId, PaginationParams @params)
    {
        var userdev = await _userep.GetByIdAsync(userId);

        if (userdev.Id == 0)
            throw new UserNotFoundException();

        var DbResult = await _repository.GetAllByIdAsync(userId, @params);

        if (DbResult.Count == 0)
            throw new SellerPostsNotFoundException();

        var dBim = await _imageRepository.GetFirstAllAsync();

        List<SellerPostViewModel> Result = new List<SellerPostViewModel>();

        foreach (var item in DbResult)
        {
            item.PostImages = new List<SellerPostImage>();
            item.AverageStars = await _sellerPostStarService.AvarageStarAsync(item.Id);
            item.UserStars = await _sellerPostStarService.GetUserStarAsync(item.Id);

            foreach (var img in dBim)
            {
                if (img.SellerPostId == item.Id)
                {
                    item.PostImages.Add(img);
                    item.MainImage = img.ImagePath;
                    dBim.RemoveAt(0);
                    break;
                }
            }

            Result.Add(item);
        }

        var DBCount = await _repository.CountAsync();
        _paginator.Paginate(DBCount, @params);

        return Result;
    }

    public async Task<List<SellerPost>> GetAllByIdAsync(long CategoryId)
    {
        var res = await _repository.GetAllByIdSellerAsync(CategoryId);

        return res;
    }

    public async Task<SellerPostViewModel> GetBYIdAsync(long sellerId)
    {
        var item = await _repository.GetByIdAsync(sellerId);
        var dBim = await _imageRepository.GetByIdAllAsync(sellerId);

        item.AverageStars = await _sellerPostStarService.AvarageStarAsync(sellerId);
        item.UserStars = await _sellerPostStarService.GetUserStarAsync(sellerId);
        if (item.Id == 0)
            throw new SellerPostsNotFoundException();

        item.PostImages = new List<SellerPostImage>();

        foreach (var img in dBim)
        {
            if (img.SellerPostId == item.Id)
            {
                item.PostImages.Add(img);
            }
        }

        return item;
    }

    public async Task<bool> ImageUpdateAsync(long posrImageId, SellerPostImageUpdateDto dto)
    {
        var DbFoundImg = await _imageRepository.GetByIdAsync(posrImageId);

        if (DbFoundImg.Id == 0)
            throw new ImageNotFoundException();

        await _fileservice.DeleteImageAsync(DbFoundImg.ImagePath);
        var img = await _fileservice.UploadImageAsync(dto.ImagePath, SELLERPOSTIMAGES);


        DbFoundImg.ImagePath = img;
        DbFoundImg.UpdatedAt = TimeHelper.GetDateTime();

        var DbResult = await _imageRepository.UpdateAsync(posrImageId, DbFoundImg);

        return DbResult > 0;
    }

    public async Task<(long IteamCount, List<SellerPostViewModel>)> SearchAsync(string search)
    {
        var res = await _repository.SearchAsync(search);

        if (res.ItemsCount == 0)
        {
            List<SellerPostViewModel> empty = new List<SellerPostViewModel>();
            return (0, empty);
        }

        var dBim = await _imageRepository.GetFirstAllAsync();

        List<SellerPostViewModel> Result = new List<SellerPostViewModel>();

        foreach (var item in res.Item2)
        {
            item.PostImages = new List<SellerPostImage>();
            item.AverageStars = await _sellerPostStarService.AvarageStarAsync(item.Id);
            item.UserStars = await _sellerPostStarService.GetUserStarAsync(item.Id);

            foreach (var img in dBim)
            {
                if (img.SellerPostId == item.Id)
                {
                    item.PostImages.Add(img);
                    item.MainImage = img.ImagePath;
                    dBim.RemoveAt(0);
                    break;
                }
            }

            Result.Add(item);
        }

        return res;
    }

    public async Task<List<PostCreatedAt>> SellerDaylilyCreatedAsync(int day)
    {
        var result = await _repository.SellerDaylilyCreatedAsync(day.ToString()); 
        
        return result;
    }

    public async Task<List<PostCreatedAt>> SellerMonthlyCreatedAsync(int month)
    {
        var result = await _repository.SellerMonthlyCreatedAsync(month.ToString());

        return result;
    }

    public async Task<bool> UpdateAsync(long sellerID, SellerPostUpdateDto dto)
    {
        var DbFound = await _repository.GetByIdAsync(sellerID);

        if (DbFound.Id == 0)
            throw new SellerPostsNotFoundException();

       /* var check = await _categoryRepository.GetByIdAsync(dto.CategoryId);
        if (check.Id == 0)
        {
            throw new CategoryNotFoundException();
        }*/

        SellerPost sellerPost = new SellerPost()
        {
            UserId = _identity.Id,
            //CategoryId = dto.CategoryId,
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Capacity = dto.Capacity,
            CapacityMeasure = dto.CapacityMeasure,
            Type = dto.Type,
            Region = dto.Region,
            District = dto.District,
            PhoneNumber = dto.PhoneNumber,
            Status = DbFound.Status,
            CreatedAt = DbFound.CreatedAt,
            UpdatedAt = TimeHelper.GetDateTime(),
        };

        var DbResult = await _repository.UpdateAsync(sellerID, sellerPost);
        if (DbResult > 0)
            return true;

        return false;
    }

    public async Task<bool> UpdateStatusAsync(long postId, SellerPostStatusUpdateDto dto)
    {
        var DbFound = await _repository.GetIdAsync(postId);

        if (DbFound.Id == 0)
            throw new SellerPostsNotFoundException();

        DbFound.Status = dto.PostStatus;
        DbFound.UpdatedAt = TimeHelper.GetDateTime();

        var DbResult = await _repository.UpdateAsync(postId, DbFound);

        if (DbResult > 0)
            return true;

        return false;
    }
}
