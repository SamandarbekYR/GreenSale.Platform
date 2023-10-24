using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Users;
using GreenSale.DataAccess.ViewModels.Users;
using GreenSale.Domain.Entites.Users;
using static Dapper.SqlMapper;

namespace GreenSale.DataAccess.Repositories.Users;

public class UserRepository : BaseRepository, IUserRepository
{
    public async Task<long> CountAsync()
    {
        try
        {
            await _connection.OpenAsync();
            string query = "Select Count(*) from public.users ;";
            var result = await _connection.QuerySingleAsync<long>(query);

            return result;
        }
        catch
        {
            return 0;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> CreateAsync(User entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "INSERT INTO public.users( first_name, last_name, phone_number, region, district, " +
                " address, password_hash, phone_number_confirm, salt, created_at, updated_at) " +
                    " VALUES ( @FirstName, @LastName, @PhoneNumber, @Region, @District, @Address, " +
                        " @PasswordHash, @PhoneNumberConfirme, @Salt, @CreatedAt, @UpdatedAt) RETURNING id ";

            var result = await _connection.ExecuteScalarAsync<int>(query, entity);

            return result;
        }
        catch
        {
            return 0;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> DeleteAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();
            string query = $"Delete  from public.users  Where id ={Id};";
            var result = await _connection.ExecuteAsync(query);

            return result;
        }
        catch
        {
            return 0;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<UserViewModel>> GetAllAsync(PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "SELECT id, first_name, last_name, phone_number, region, district, address , created_at, updated_at " +
                $"FROM public.users ORDER BY id DESC OFFSET {@params.GetSkipCount()} LIMIT {@params.PageSize};";

            var result = (await _connection.QueryAsync<UserViewModel>(query)).ToList();

            return result.ToList();
        }
        catch
        {
            return new List<UserViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<UserViewModel> GetByIdAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "SELECT id, first_name, last_name, phone_number, region, district, address, created_at, updated_at " +
                $" FROM public.users where id = {Id};";

            var result = await _connection.QuerySingleOrDefaultAsync<UserViewModel>(query, new { id = Id });

            return result;
        }
        catch
        {
            return new UserViewModel();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<User> GetByPhoneAsync(string phone)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "SELECT * FROM public.users where phone_number = @PhoneNumber; ";

            var result = await _connection.QuerySingleAsync<User>(query, new { PhoneNumber = phone });

            return result;
        }
        catch
        {
            return null;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<(int ItemsCount, List<UserViewModel>)> SearchAsync(string search)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"SELECT *  FROM user_role_viewmodel  WHERE (first_name  ILIKE '%{search}%' or last_name ILIKE '%{search}%')" +
                " and role_name = 'User'";

            /*var parameters = new
            {
                Search = search,
                offset = @params.PageNumber * @params.PageSize,
                limit = @params.PageSize
            };*/

            var result = await _connection.QueryAsync<UserViewModel>(query);
            int Count = result.Count();

            return (Count, result.ToList());
        }
        catch
        {
            return (0, new List<UserViewModel>());
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> UpdateAsync(long Id, User entity)
    {
        try
        {
            await _connection.OpenAsync();
            string query = "UPDATE public.users SET first_name=@FirstName, last_name= @Lastname, phone_number=@PhoneNumber, " +
                "region=@Region, district=@District, address=@Address, password_hash=@PasswordHash, " +
                    "phone_number_confirm=@PhoneNumberConfirme, salt=@Salt,  updated_at=@UpdatedAt " +
                        $"WHERE id={Id} RETURNING id ";

            var result = await _connection.ExecuteScalarAsync<int>(query, entity);

            return result;
        }
        catch
        {
            return 0;
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }
}
