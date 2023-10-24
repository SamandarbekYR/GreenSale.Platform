using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Storages;
using GreenSale.Domain.Entites.Storages;

namespace GreenSale.DataAccess.Repositories.Storages
{

    public class StorageStarRepository : BaseRepository, IStorageStarRepository
    {
        public async Task<long> CountAsync()
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT COUNT(*) FROM storagepoststars;";
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

        public async Task<int> CreateAsync(StoragePostStars entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "INSERT INTO public.storagepoststars( " +
                    "user_id, post_id, stars, created_at, updated_at) " +
                        "VALUES(@UserId, @PostId, @Stars, @CreatedAt, @UpdatedAt); ";

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

                string query = $"DELETE FROM public.storagepoststars WHERE post_id = @ID;";
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

        public async Task<bool> DeleteUserAsync(long userId)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"DELETE FROM public.storages WHERE user_id = @ID;";
                var result = await _connection.ExecuteAsync(query, new { ID = userId });

                return result > 0;
            }
            catch
            {
                return false;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<StoragePostStars>> GetAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT * FROM storagepoststars order by id desc " +
                    $"offset {@params.GetSkipCount()} limit {@params.PageSize}";

                var result = (await _connection.QueryAsync<StoragePostStars>(query)).ToList();

                return result;
            }
            catch
            {
                return new List<StoragePostStars>() { };
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<int>> GetAllStarsPostIdCountAsync(long postid)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT stars FROM storagepoststars where post_id=@Id;";
                var result = (await _connection.QueryAsync<int>(query, new { Id = postid })).ToList();

                return result;
            }
            catch
            {
                return new List<int>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<StoragePostStars> GetByIdAsync(long Id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM public.storagepoststars where id=@ID;";
                var result = await _connection.QuerySingleAsync<StoragePostStars>(query, new { ID = Id });

                return result;
            }
            catch
            {
                return new StoragePostStars();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<long> GetIdAsync(long userid, long postid)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT id as Id FROM public.storagepoststars where user_id=@USERID and post_id = @POSTID;";
                var result = await _connection.QuerySingleAsync<long>(query, new { USERID = userid, POSTID = postid });

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

        public async Task<int> UpdateAsync(long Id, StoragePostStars entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"UPDATE public.storagepoststars " +
                    $"SET stars = @Stars, created_at = @CreatedAt, updated_at = @UpdatedAt " +
                        $"WHERE id={Id}; ";

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
    }
}