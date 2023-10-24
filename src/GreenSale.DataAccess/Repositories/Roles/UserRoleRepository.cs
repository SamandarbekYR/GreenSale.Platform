using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Roles;
using GreenSale.DataAccess.ViewModels.UserRoles;
using GreenSale.Domain.Entites.Roles.UserRoles;

namespace GreenSale.DataAccess.Repositories.Roles;

public class UserRoleRepository : BaseRepository, IUserRoles
{
    public async Task<long> CountAsync()
    {
        try
        {
            await _connection.OpenAsync();
            string query = "select count(*) from user_roles where role_id = 1";
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

    public async Task<int> CreateAsync(UserRole entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "insert into user_roles(user_id, role_id, created_at, updated_at) " +
                "values(@UserId, @RoleId, @CreatedAt, @UpdatedAt) RETURNING id ";

            var result = await _connection.ExecuteAsync(query, entity);

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
            string query = "Delete from user_roles where id = @ID or user_id = @ID";
            var result = await _connection.ExecuteAsync(query, new { ID = Id });

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

    public async Task<List<long>> GetAdminIdASync(PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string qauery = "SELECT user_id FROM user_roles where role_id=2 order by id desc " +
                $"offset {@params.GetSkipCount()} limit {@params.PageSize} ";

            var result = (await _connection.QueryAsync<long>(qauery)).ToList();

            return result;
        }
        catch
        {
            return new List<long>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<UserRoleViewModel>> GetAllAsync(PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string qauery = "SELECT * FROM user_role_viewmodel order by id desc " +
                $"offset {@params.GetSkipCount()} limit {@params.PageSize} ";

            var result = (await _connection.QueryAsync<UserRoleViewModel>(qauery)).ToList();

            return result;
        }
        catch
        {
            return new List<UserRoleViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<UserRoleViewModel> GetByIdAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();
            string query = "select * from user_role_viewmodel  where id = @ID";
            var result = await _connection.QuerySingleAsync<UserRoleViewModel>(query, new { Id = Id });

            return result;
        }
        catch
        {
            return new UserRoleViewModel();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<long>> GetUserIdASync(PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string qauery = "SELECT user_id FROM user_roles where role_id = 1 order by id desc " +
                $"offset {@params.GetSkipCount()} limit {@params.PageSize} ";

            var result = (await _connection.QueryAsync<long>(qauery)).ToList();

            return result;
        }
        catch
        {
            return new List<long>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<UserRole> GetUserRole(long id)
    {
        try
        {
            await _connection.OpenAsync();
            string query = "select role_id from user_roles  where user_id = @ID";
            var result = await _connection.QuerySingleAsync<UserRole>(query, new { Id = id });

            return result;
        }
        catch
        {
            return new UserRole();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> UpdateAsync(long Id, UserRole entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"UPDATE user_roles " +
                "SET  role_id = @RoleId,  updated_at = @UpdatedAt " +
                    $"WHERE user_id ={Id} RETURNING id ";

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