using GreenSale.Application.Exceptions.BuyerPosts;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.BuyerPosts;
using GreenSale.Domain.Entites.BuyerPosts;
using GreenSale.Persistence.Dtos.BuyerPostsDto;
using GreenSale.Service.Interfaces.Auth;
using GreenSale.Service.Interfaces.BuyerPosts;

namespace GreenSale.Service.Service.BuyerPosts;

public class BuyerPostStarService : IBuyerPostStarService
{
    public readonly IBuyerPostStarRepository _buyerPostStarRepository;
    public readonly IBuyerPostRepository _buyerPostRepository;
    public readonly IIdentityService _identityService;

    public BuyerPostStarService(IBuyerPostStarRepository buyerPostStarRepository,
        IBuyerPostRepository buyerPostRepository,
        IIdentityService identityService)
    {
        this._buyerPostStarRepository = buyerPostStarRepository;
        this._buyerPostRepository = buyerPostRepository;
        this._identityService = identityService;
    }

    public async Task<long> CountAsync()
        => await _buyerPostStarRepository.CountAsync();

    public async Task<bool> CreateAsync(BuyerPostStarCreateDto dto)
    {
        BuyerPostStars stars = new BuyerPostStars();
        stars.UserId = _identityService.Id;
        stars.PostId =dto.PostId;

        var post = await _buyerPostRepository.GetByIdAsync(dto.PostId);
        if (post.Id == 0)
        {
            throw new BuyerPostNotFoundException();
        }
        else
        {
            long Id = await GetIdAsync(stars.UserId, stars.PostId);

            if (Id == 0)
            {
                stars.Stars = dto.Stars;
                stars.CreatedAt = Helpers.TimeHelper.GetDateTime();
                stars.UpdatedAt = Helpers.TimeHelper.GetDateTime();

                var result = await _buyerPostStarRepository.CreateAsync(stars);

                return result>0;
            }
            else
            {
                var starsOld = await _buyerPostStarRepository.GetByIdAsync(Id);

                BuyerPostStars starsNew = new BuyerPostStars();
                starsNew.UserId = starsOld.UserId;
                starsNew.PostId = starsOld.PostId;
                starsNew.Stars = dto.Stars;
                starsNew.CreatedAt = starsOld.CreatedAt;
                starsNew.UpdatedAt = Helpers.TimeHelper.GetDateTime();

                var result = await _buyerPostStarRepository.UpdateAsync(Id, starsNew);

                return result>0;
            }
        }
    }

    public async Task<bool> DeleteAsync(long userId, long postId)
    {
        var result = await _buyerPostStarRepository.DeleteAsync(postId);

        return result > 0;
    }

    public Task<List<BuyerPostStars>> GetAllAsync(PaginationParams @params)
    {
        throw new NotImplementedException();
    }

    public async Task<BuyerPostStars> GetByIdAsync(long Id)
    {
        long UserId = _identityService.Id;
        long RowId = await GetIdAsync(UserId, Id);

        var result = await _buyerPostStarRepository.GetByIdAsync(RowId);

        return result;
    }

    public async Task<bool> UpdateAsync(long PostId, BuyerPostStarUpdateDto dto)
    {
        long UserId=_identityService.Id;

        var post = await _buyerPostRepository.GetByIdAsync(PostId);
        if (post.Id == 0)
        {
            throw new BuyerPostNotFoundException();
        }
        else
        {
            long Id = await GetIdAsync(UserId, PostId);

            var starsOld = await _buyerPostStarRepository.GetByIdAsync(Id);

            BuyerPostStars starsNew = new BuyerPostStars();
            starsNew.UserId = starsOld.UserId;
            starsNew.PostId = starsOld.PostId;
            starsNew.Stars = dto.Stars;
            starsNew.CreatedAt = starsOld.CreatedAt;
            starsNew.UpdatedAt = Helpers.TimeHelper.GetDateTime();

            var result = await _buyerPostStarRepository.UpdateAsync(Id, starsNew);

            return result>0;
        }
    }

    public async Task<long> GetIdAsync(long userid, long postid)
        => await _buyerPostStarRepository.GetIdAsync(userid, postid);

    public async Task<double> AvarageStarAsync(long postid)
    {
        List<int> starlist = await _buyerPostStarRepository.GetAllStarsPostIdCountAsync(postid);
        double avaragestar = 0;
        if (starlist.Count == 0)
        {
            return avaragestar;
        }
        else
        {
            long totalstar = 0;
            foreach (var star in starlist)
            {
                totalstar += star;
            }
            avaragestar=Convert.ToDouble(totalstar)/starlist.Count;
            return Math.Round(avaragestar,2);
        }
    }

    public async Task<int> GetUserStarAsync(long postId)
    {
        long userid = _identityService.Id;
        long Id = await GetIdAsync(userid, postId);
        int userstar = 0;
        if (Id == 0)
        {
            return userstar;
        }
        else
        {
            var userpoststar =await _buyerPostStarRepository.GetByIdAsync(Id);
            userstar = userpoststar.Stars;

            return userstar;
        }
    }

    public Task<bool> DeleteUserAsync(long userId)
    {
        throw new NotImplementedException();
    }
}
