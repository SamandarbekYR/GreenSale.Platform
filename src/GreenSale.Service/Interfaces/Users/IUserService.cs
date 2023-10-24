using GreenSale.Application.Utils;
using GreenSale.DataAccess.ViewModels.Storages;
using GreenSale.DataAccess.ViewModels.Users;
using GreenSale.Persistence.Dtos.UserDtos;
using GreenSale.Persistence.Validators.Users;

namespace GreenSale.Service.Interfaces.Users;

public interface IUserService
{
    public Task<bool> DeleteAsync(long userId);
    public Task<bool> UpdateAsync(UserUpdateDto dto);
    public Task<bool> UpdateSecuryAsync(UserSecurityUpdate dto);
    // public Task<bool> UpdateByAdminAsync(long userId, UserUpdateDto dto);
    public Task<long> CountAsync();
    public Task<List<UserViewModel>> GetAllAsync(PaginationParams @params);
    public Task<UserViewModel> GetByIdAsync(long userId);
    public Task<List<UserViewModel>> GetAllAdminAsync(PaginationParams @params);
    public Task<List<UserViewModel>> GetAllUserAsync(PaginationParams @params);
    public Task<(long IteamCount, List<UserViewModel>)> SearchAsync(string search);
}
