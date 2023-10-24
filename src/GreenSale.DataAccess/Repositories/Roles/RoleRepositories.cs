using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Roles;
using GreenSale.Domain.Entites.Roles;

namespace GreenSale.DataAccess.Repositories.Roles
{
    public class RoleRepositories : BaseRepository, IRoleRepository
    {
        public async Task<long> CountAsync()
        {
            try
            {
                await _connection.OpenAsync();
                string query = "select count(*) from roles";
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

        public async Task<int> CreateAsync(Role entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "insert into roles(name, created_at, updated_at) " +
                    "values (@Name, @CreatedAt, @UpdatedAt ) RETURNING id ";

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
                string query = "Delete from roles where id = @ID;";
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

        public async Task<List<Role>> GetAllAsync(PaginationParams @params)
        {
            try
            {

                await _connection.OpenAsync();

                string query = "SELECT * FROM Role order by id desc " +
                    $"offset {@params.GetSkipCount()} limit {@params.PageSize} ";

                var result = (await _connection.QueryAsync<Role>(query)).ToList();

                return result;
            }
            catch
            {
                return new List<Role>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Role> GetByIdAsync(long Id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"select * from roles where id =@ID";
                var result = await _connection.QuerySingleAsync<Role>(query, new { ID = Id });

                return result;
            }
            catch
            {
                return new Role();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<int> UpdateAsync(long Id, Role entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "UPDATE public.categories " +
                    $"SET name=@Name, created_at=@CreatedAt, updated_at=@UpdatedAt WHERE id={Id};";

                var result = await _connection.QuerySingleAsync(query, entity);

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
}
