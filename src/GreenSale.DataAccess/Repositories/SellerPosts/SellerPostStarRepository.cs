using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.SellerPosts;
using GreenSale.Domain.Entites.SellerPosts;

namespace GreenSale.DataAccess.Repositories.SellerPosts
{

    public class SellerPostStarRepository : BaseRepository, ISellerPostStarRepository
    {
        public async Task<long> CountAsync()
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT COUNT(*) FROM sellerpoststars;";
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

        public async Task<int> CreateAsync(SellerPostStars entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "INSERT INTO public.sellerpoststars( " +
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

                string query = $"DELETE FROM public.sellerpoststars WHERE post_id = @ID;";
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

                string query = $"DELETE FROM public.sellerpoststars WHERE user_id = @ID;";
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

        public async Task<List<SellerPostStars>> GetAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT * FROM sellerpoststars order by id desc " +
                    $"offset {@params.GetSkipCount()} limit {@params.PageSize}";

                var result = (await _connection.QueryAsync<SellerPostStars>(query)).ToList();

                return result;
            }
            catch
            {
                return new List<SellerPostStars>() { };
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

                string query = $"SELECT stars as Stars FROM sellerpoststars where post_id=@Id;";
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

        public async Task<SellerPostStars> GetByIdAsync(long Id)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"SELECT * FROM public.sellerpoststars where id=@ID;";
                var result = await _connection.QuerySingleAsync<SellerPostStars>(query, new { ID = Id });

                return result;
            }
            catch
            {
                return new SellerPostStars();
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

                string query = $"SELECT id as Id FROM public.sellerpoststars where user_id=@USERID and post_id = @POSTID;";
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

        public async Task<int> UpdateAsync(long Id, SellerPostStars entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"UPDATE public.sellerpoststars " +
                    $"SET stars = @Stars, created_at = @CreatedAt, updated_at = @UpdatedAt " +
                        $"WHERE id = {Id};";

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