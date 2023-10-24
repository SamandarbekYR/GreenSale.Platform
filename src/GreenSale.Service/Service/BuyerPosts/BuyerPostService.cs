using GreenSale.Application.Exceptions;
using GreenSale.Application.Exceptions.BuyerPosts;
using GreenSale.Application.Exceptions.Categories;
using GreenSale.Application.Exceptions.Users;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.BuyerPosts;
using GreenSale.DataAccess.Interfaces.Categories;
using GreenSale.DataAccess.Interfaces.Users;
using GreenSale.DataAccess.ViewModels.BuyerPosts;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.Domain.Entites.BuyerPosts;
using GreenSale.Persistence.Dtos.BuyerPostImageUpdateDtos;
using GreenSale.Persistence.Dtos.BuyerPostsDto;
using GreenSale.Service.Helpers;
using GreenSale.Service.Interfaces.Auth;
using GreenSale.Service.Interfaces.BuyerPosts;
using GreenSale.Service.Interfaces.Common;

namespace GreenSale.Service.Service.BuyerPosts;

public class BuyerPostService : IBuyerPostService
{
    private readonly IUserRepository _userep;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IBuyerPostRepository _postRepository;
    private readonly IPaginator _paginator;
    private readonly IFileService _fileService;
    private readonly IBuyerPostImageRepository _imageRepository;
    private readonly IBuyerPostStarService _buyerPostStarService;
    private readonly IIdentityService _identity;
    private readonly string BUYERPOSTIMAGES = "BuyerPostImages";

    public BuyerPostService(
        IBuyerPostRepository postRepository,
        IPaginator paginator,
        IFileService fileService,
        IBuyerPostImageRepository imageRepository,
        IIdentityService identity,
        ICategoryRepository categoryRepository,
        IBuyerPostStarService buyerPostStarService,
        IUserRepository userRepository)
    {
        this._userep = userRepository;
        this._categoryRepository = categoryRepository;
        this._postRepository = postRepository;
        this._paginator = paginator;
        this._fileService = fileService;
        this._imageRepository = imageRepository;
        this._buyerPostStarService = buyerPostStarService;
        this._identity = identity;
    }

    public async Task<List<PostCreatedAt>> BuyerDaylilyCreatedAsync(int day)
    {
        var result = await _postRepository.BuyerDaylilyCreatedAsync(day.ToString()); 
        
        return result;
    }

    public async Task<List<PostCreatedAt>> BuyerMonthlyCreatedAsync(int month)
    {
        var result = await _postRepository.BuyerDaylilyCreatedAsync(month.ToString());

        return result;
    }

    public async Task<long> CountAsync()
    {
        var DbResult = await _postRepository.CountAsync();

        return DbResult;
    }

    public async Task<long> CountStatusAgreeAsync()
    {
        var DbResult = await _postRepository.CountStatusAgreeAsync();

        return DbResult;
    }

    public async Task<long> CountStatusNewAsync()
    {
        var DbResult = await _postRepository.CountStatusNewAsync();

        return DbResult;
    }

    public async Task<bool> CreateAsync(BuyerPostCreateDto dto)
    {
        var check = await _categoryRepository.GetByIdAsync(dto.CategoryID);
        if (check.Id == 0)
        {
            throw new CategoryNotFoundException();
        }

        BuyerPost buyerPost = new BuyerPost()
        {
            UserId = _identity.Id,
            CategoryID = dto.CategoryID,
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Capacity = dto.Capacity,
            CapacityMeasure = dto.CapacityMeasure,
            Type = dto.Type,
            Region = dto.Region,
            Address = dto.Address,
            District = dto.District,
            PhoneNumber = dto.PhoneNumber,
            Status = Domain.Enums.BuyerPosts.BuyerPostEnum.New,
            CreatedAt = TimeHelper.GetDateTime(),
            UpdatedAt = TimeHelper.GetDateTime(),
        };

        var DbResult = await _postRepository.CreateAsync(buyerPost);

        if (DbResult > 0)
        {
            foreach (var item in dto.ImagePath)
            {
                var img = await _fileService.UploadImageAsync(item, BUYERPOSTIMAGES);

                BuyerPostImage BuyerPostImage = new BuyerPostImage()
                {
                    BuyerpostId = DbResult,
                    ImagePath = img,
                    CreatedAt = TimeHelper.GetDateTime(),
                    UpdatedAt = TimeHelper.GetDateTime(),
                };

                var DbImgResult = await _imageRepository.CreateAsync(BuyerPostImage);
            }

            return true;
        }

        return false;
    }

    public async Task<bool> DeleteAsync(long buyerId)
    {
        var DbFound = await _postRepository.GetByIdAsync(buyerId);

        if (DbFound.Id == 0)
            throw new BuyerPostNotFoundException();

        var DbImgAll = await _imageRepository.GetByIdAllAsync(buyerId);

        /*if (DbImgAll.Count == 0)
            throw new ImageNotFoundException();*/

        var DbImgResult = await _imageRepository.DeleteAsync(buyerId);
        var deletestarresult = await _buyerPostStarService.DeleteAsync(DbFound.UserId, buyerId);
        var DbResult = await _postRepository.DeleteAsync(buyerId);

        if (DbResult > 0 && 0 < DbImgResult && deletestarresult)
        {
            if(DbImgAll.Count != 0)
            {
                foreach (var item in DbImgAll)
                {
                    await _fileService.DeleteImageAsync(item.ImagePath);
                }
            }
            
        }

        return DbResult > 0;
    }

    public async Task<bool> DeleteImageIdAsync(long ImageId)
    {
        var DbFound = await _imageRepository.GetByIdAsync(ImageId);
        if (DbFound.Id != 0)
        {
            var Result = await _imageRepository.DeleteAsync(ImageId);
            await _fileService.DeleteImageAsync(DbFound.ImagePath);
            return Result > 0;
        }
        throw new ImageNotFoundException();
    }

    public async Task<List<BuyerPostViewModel>> GetAllAsync(PaginationParams @params)
    {
        var DbResult = await _postRepository.GetAllAsync(@params);
        var dBim = await _imageRepository.GetFirstAllAsync();

        List<BuyerPostViewModel> Result = new List<BuyerPostViewModel>();

        foreach (var item in DbResult)
        {
            item.BuyerPostsImages = new List<BuyerPostImage>();
            item.AverageStars = await _buyerPostStarService.AvarageStarAsync(item.Id);
            item.UserStars=await _buyerPostStarService.GetUserStarAsync(item.Id);

            foreach (var img in dBim)
            {
                if (img.BuyerpostId == item.Id)
                {
                    item.BuyerPostsImages.Add(img);
                    item.MainImage = img.ImagePath;
                    dBim.RemoveAt(0);
                    break;
                }
            }

            Result.Add(item);
        }

        var DBCount = await _postRepository.CountAsync();
        _paginator.Paginate(DBCount, @params);

        return Result;
    }

    public async Task<List<BuyerPostViewModel>> GetAllByIdAsync(long userId, PaginationParams @params)
    {
        var userdev = await _userep.GetByIdAsync(userId);

        if (userdev is null)
            throw new UserNotFoundException();

        var DbResult = await _postRepository.GetAllByIdAsync(userId, @params);

        if (DbResult.Count == 0)
            throw new BuyerPostNotFoundException();

        var dBim = await _imageRepository.GetFirstAllAsync();

        List<BuyerPostViewModel> Result = new List<BuyerPostViewModel>();

        foreach (var item in DbResult)
        {
            item.BuyerPostsImages = new List<BuyerPostImage>();
            item.AverageStars = await _buyerPostStarService.AvarageStarAsync(item.Id);
            item.UserStars = await _buyerPostStarService.GetUserStarAsync(item.Id);
            foreach (var img in dBim)
            {
                if (img.BuyerpostId == item.Id)
                {
                    item.BuyerPostsImages.Add(img);
                    item.MainImage = img.ImagePath;
                    dBim.RemoveAt(0);
                    break;
                }
            }

            Result.Add(item);
        }

        var DBCount = await _postRepository.CountAsync();
        _paginator.Paginate(DBCount, @params);

        return Result;
    }

    public async Task<List<BuyerPostViewModel>> GetAllByIdAsync(long BuyerId)
    {
        var res = await _postRepository.GetAllByIdBuyerAsync(BuyerId);
        foreach (var item in res)
        {
            item.AverageStars = await _buyerPostStarService.AvarageStarAsync(item.Id);
            item.UserStars = await _buyerPostStarService.GetUserStarAsync(item.Id);
        }
        return res;
    }

    public async Task<BuyerPostViewModel> GetBYIdAsync(long buyerId)
    {
        var item = await _postRepository.GetByIdAsync(buyerId);
        var dBim = await _imageRepository.GetByIdAllAsync(buyerId);

        if (item.Id == 0)
            throw new BuyerPostNotFoundException();

        item.AverageStars = await _buyerPostStarService.AvarageStarAsync(buyerId);
        item.UserStars = await _buyerPostStarService.GetUserStarAsync(buyerId);

        item.BuyerPostsImages = new List<BuyerPostImage>();

        foreach (var img in dBim)
        {
            if (img.BuyerpostId == item.Id)
            {
                item.BuyerPostsImages.Add(img);
            }
        }

        return item;
    }

    public async Task<bool> ImageUpdateAsync(long ImageId, BuyerPostImageDto dto)
    {
        var DbFoundImg = await _imageRepository.GetByIdAsync(ImageId);

        if (DbFoundImg.Id == 0)
            throw new ImageNotFoundException();

        await _fileService.DeleteImageAsync(DbFoundImg.ImagePath);
        var img = await _fileService.UploadImageAsync(dto.ImagePath, BUYERPOSTIMAGES);

        DbFoundImg.ImagePath = img;
        DbFoundImg.UpdatedAt = TimeHelper.GetDateTime();

        var DbResult = await _imageRepository.UpdateAsync(ImageId, DbFoundImg);

        return DbResult > 0;
    }

    public async Task<(long IteamCount, List<BuyerPostViewModel>)> SearchingAsync(string search)
    {
        var DbResult = await _postRepository.SearchAsync(search);

        if (DbResult.ItemsCount == 0)
        {
            List<BuyerPostViewModel> empty = new List<BuyerPostViewModel>();
            return (0, empty);
        }

        var dBim = await _imageRepository.GetFirstAllAsync();

        List<BuyerPostViewModel> Result = new List<BuyerPostViewModel>();

        foreach (var item in DbResult.Item2)
        {
            item.AverageStars = await _buyerPostStarService.AvarageStarAsync(item.Id);
            item.UserStars = await _buyerPostStarService.GetUserStarAsync(item.Id);
            item.BuyerPostsImages = new List<BuyerPostImage>();

            foreach (var img in dBim)
            {
                if (img.BuyerpostId == item.Id)
                {
                    item.BuyerPostsImages.Add(img);
                    item.MainImage = img.ImagePath;
                    dBim.RemoveAt(0);
                    break;
                }
            }

            Result.Add(item);
        }

        return (DbResult.ItemsCount, DbResult.Item2);
    }

    public async Task<bool> UpdateAsync(long buyerID, BuyerPostUpdateDto dto)
    {
        var DbFound = await _postRepository.GetByIdAsync(buyerID);

        if (DbFound.Id == 0)
            throw new BuyerPostNotFoundException();

       /* var check = await _categoryRepository.GetByIdAsync(dto.CategoryID);
        if (check.Id == 0)
        {
            throw new CategoryNotFoundException();
        }*/

        BuyerPost buyerPost = new BuyerPost()
        {
            UserId = _identity.Id,
            //CategoryID = dto.CategoryID,
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            Capacity = dto.Capacity,
            CapacityMeasure = dto.CapacityMeasure,
            Type = dto.Type,
            Region = dto.Region,
            Address = dto.Address,
            District = dto.District,
            PhoneNumber = dto.PhoneNumber,
            CreatedAt = DbFound.CreatedAt,
            UpdatedAt = TimeHelper.GetDateTime(),
        };

        var DbResult = await _postRepository.UpdateAsync(buyerID, buyerPost);

        if (DbResult > 0)
            return true;

        return false;
    }

    public async Task<bool> UpdateStatusAsync(long buyerID, BuyerPostStatusUpdateDto dto)
    {
        var DbFound = await _postRepository.GetIdAsync(buyerID);

        if (DbFound.Id == 0)
            throw new BuyerPostNotFoundException();

        DbFound.Status = dto.PostStatus;
        DbFound.UpdatedAt = TimeHelper.GetDateTime();

        var DbResult = await _postRepository.UpdateAsync(buyerID, DbFound);

        if (DbResult > 0)
            return true;

        return false;
    }
}
