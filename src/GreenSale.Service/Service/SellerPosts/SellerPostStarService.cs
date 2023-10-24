using GreenSale.Application.Exceptions.SellerPosts;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.SellerPosts;
using GreenSale.Domain.Entites.SellerPosts;
using GreenSale.Persistence.Dtos.SellerPostsDtos;
using GreenSale.Service.Interfaces.Auth;
using GreenSale.Service.Interfaces.SellerPosts;

namespace GreenSale.Service.Service.SellerPosts;

public class SellerPostStarService : ISellerPostStarService
{
    public readonly ISellerPostStarRepository _sellerPostStarRepository;
    public readonly ISellerPostsRepository _sellerPostsRepository;
    public readonly IIdentityService _identityService;

    public SellerPostStarService(ISellerPostStarRepository sellerPostStarRepository,
        ISellerPostsRepository sellerPostsRepository,
        IIdentityService identityService)
    {
        this._sellerPostStarRepository = sellerPostStarRepository;
        this._sellerPostsRepository = sellerPostsRepository;
        this._identityService = identityService;
    }

    public async Task<long> CountAsync()
        => await _sellerPostStarRepository.CountAsync();

    public async Task<bool> CreateAsync(SellerPostStarCreateDto dto)
    {
        SellerPostStars stars = new SellerPostStars();
        stars.UserId = _identityService.Id;
        stars.PostId = dto.PostId;

        var post = await _sellerPostsRepository.GetByIdAsync(dto.PostId);
        if (post.Id == 0)
        {
            throw new SellerPostsNotFoundException();
        }
        else
        {
            long Id = await GetIdAsync(stars.UserId, stars.PostId);

            if (Id == 0)
            {
                stars.Stars = dto.Stars;
                stars.CreatedAt = Helpers.TimeHelper.GetDateTime();
                stars.UpdatedAt = Helpers.TimeHelper.GetDateTime();

                var result = await _sellerPostStarRepository.CreateAsync(stars);

                return result>0;
            }
            else
            {
                var starsOld = await _sellerPostStarRepository.GetByIdAsync(Id);

                SellerPostStars starsNew = new SellerPostStars();
                starsNew.UserId = starsOld.UserId;
                starsNew.PostId = starsOld.PostId;
                starsNew.Stars = dto.Stars;
                starsNew.CreatedAt = starsOld.CreatedAt;
                starsNew.UpdatedAt = Helpers.TimeHelper.GetDateTime();

                var result = await _sellerPostStarRepository.UpdateAsync(Id, starsNew);

                return result>0;
            }
        }
    }

    public async Task<bool> DeleteAsync( long userId, long postId)
    { 
        var result = await _sellerPostStarRepository.DeleteAsync(postId);

        return result>0;
    }

    public Task<List<SellerPostStars>> GetAllAsync(PaginationParams @params)
    {
        throw new NotImplementedException();
    }

    public async Task<SellerPostStars> GetByIdAsync(long Id)
    {
        long UserId = _identityService.Id;
        long RowId = await GetIdAsync(UserId, Id);

        var result = await _sellerPostStarRepository.GetByIdAsync(RowId);

        return result;
    }

    public async Task<bool> UpdateAsync(long PostId, SellerPostStarUpdateDto dto)
    {
        long UserId = _identityService.Id;
        var post =await _sellerPostsRepository.GetByIdAsync(PostId);
        if (post.Id == 0)
        {
            throw new SellerPostsNotFoundException();
        }
        else
        {
            long Id = await GetIdAsync(UserId, PostId);

            var starsOld = await _sellerPostStarRepository.GetByIdAsync(Id);

            SellerPostStars starsNew = new SellerPostStars();
            starsNew.UserId = starsOld.UserId;
            starsNew.PostId = starsOld.PostId;
            starsNew.Stars = dto.Stars;
            starsNew.CreatedAt = starsOld.CreatedAt;
            starsNew.UpdatedAt = Helpers.TimeHelper.GetDateTime();

            var result = await _sellerPostStarRepository.UpdateAsync(Id, starsNew);

            return result>0;
        }
    }

    public async Task<long> GetIdAsync(long userid, long postid)
        => await _sellerPostStarRepository.GetIdAsync(userid, postid);

    public async Task<double> AvarageStarAsync(long postid)
    {
        List<int> starlist = await _sellerPostStarRepository.GetAllStarsPostIdCountAsync(postid);
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
            avaragestar = Convert.ToDouble(totalstar) / starlist.Count;
            return Math.Round(avaragestar, 2);
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
            var userpoststar = await _sellerPostStarRepository.GetByIdAsync(Id);
            userstar = userpoststar.Stars;

            return userstar;
        }
    }
}
