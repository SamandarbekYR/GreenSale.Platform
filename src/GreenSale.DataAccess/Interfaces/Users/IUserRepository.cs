using GreenSale.DataAccess.Common;
using GreenSale.DataAccess.ViewModels.Users;
using GreenSale.Domain.Entites.Users;

namespace GreenSale.DataAccess.Interfaces.Users
{
    public interface IUserRepository : IRepository<User, UserViewModel>, ISearchable<UserViewModel>
    {
        public Task<User> GetByPhoneAsync(string phone);
    }
}