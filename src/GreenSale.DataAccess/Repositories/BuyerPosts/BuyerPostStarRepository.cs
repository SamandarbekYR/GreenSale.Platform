using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.BuyerPosts;
using GreenSale.Domain.Entites.BuyerPosts;

namespace GreenSale.DataAccess.Repositories.BuyerPosts
{

    public class BuyerPostStarRepository : BaseRepository, IBuyerPostStarRepository
    {
        public async Task<long> CountAsync()
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT COUNT(*) FROM buyerpoststars;";
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

        public async Task<int> CreateAsync(BuyerPostStars entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "INSERT INTO public.buyerpoststars( " +
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

                string query = $"DELETE FROM public.buyerpoststars WHERE post_id = @ID;";
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

                string query = $"DELETE FROM public.buyerpoststars WHERE user_id = @ID;";
                var result = await _connection.ExecuteAsync(query, new { ID = userId });

                return result>0;
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

        public async Task<List<BuyerPostStars>> GetAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT * FROM buyerpoststars order by id desc " +
                    $"offset {@params.GetSkipCount()} limit {@params.PageSize}";

                var result = (await _connection.QueryAsync<BuyerPostStars>(query)).ToList();

                return result;
            }
            catch
            {
                return new List<BuyerPostStars>() { };
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

                string query = $"SELECT stars as Stars FROM buyerpoststars where post_id=@Id;";
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

        public async Task<BuyerPostStars> GetByIdAsync(long Id)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT * FROM public.buyerpoststars where id=@ID;";
                var result = await _connection.QuerySingleAsync<BuyerPostStars>(query, new { ID = Id });

                return result;
            }
            catch
            {
                return new BuyerPostStars();
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

                string query = $"SELECT id as Id FROM public.buyerpoststars where user_id=@USERID and post_id = @POSTID;";
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

        public async Task<int> UpdateAsync(long Id, BuyerPostStars entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"UPDATE public.buyerpoststars " +
                    $"SET stars = @Stars, created_at = @CreatedAt, updated_at = @UpdatedAt " +
                        $"WHERE id = {Id}; ";

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