using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.BuyerPosts;
using GreenSale.Domain.Entites.BuyerPosts;

namespace GreenSale.DataAccess.Repositories.BuyerPosts
{
    public class BuyerPostImageRepository : BaseRepository, IBuyerPostImageRepository
    {
        public async Task<long> CountAsync()
        {
            try
            {
                await _connection.OpenAsync();
                string query = "select count(*) from buyer_posts_images";
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

        public async Task<int> CreateAsync(BuyerPostImage entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "Insert into buyer_posts_images(buyer_post_id, image_path, created_at, updated_at) " +
                    "values(@BuyerPostId, @ImagePath, @CreatedAt, @UpdatedAt) RETURNING id ";

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
                string query = "Delete from buyer_posts_images where buyer_post_id = @ID or id = @ID";
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

        public async Task<List<BuyerPostImage>> GetAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();

                string qauery = "SELECT * FROM buyer_posts_images order by id desc " +
                                    $"offset {@params.GetSkipCount()} limit {@params.PageSize} ";

                var result = (await _connection.QueryAsync<BuyerPostImage>(qauery)).ToList();

                return result;
            }
            catch
            {
                return new List<BuyerPostImage>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<BuyerPostImage>> GetByIdAllAsync(long Id)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "select * from buyer_posts_images  where buyer_post_id = @ID";
                var result = (await _connection.QueryAsync<BuyerPostImage>(query, new { ID = Id })).ToList();

                return result;
            }
            catch
            {
                return new List<BuyerPostImage>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<BuyerPostImage> GetByIdAsync(long Id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "select * from buyer_posts_images  where id = @ID";
                var result = await _connection.QuerySingleAsync<BuyerPostImage>(query, new { Id = Id });

                return result;
            }
            catch
            {
                return new BuyerPostImage();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<List<BuyerPostImage>> GetFirstAllAsync()
        {
            try
            {
                await _connection.OpenAsync();

                string qauery = "SELECT id, buyer_post_id, image_path, created_at, updated_at " +
                    "FROM public.buyer_posts_images WHERE (buyer_post_id, id) " +
                        "IN (SELECT buyer_post_id, MIN(id) FROM public.buyer_posts_images " +
                            "GROUP BY buyer_post_id ) ORDER BY id DESC";

                var result = (await _connection.QueryAsync<BuyerPostImage>(qauery)).ToList();

                return result;
            }
            catch
            {
                return new List<BuyerPostImage>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<int> UpdateAsync(long Id, BuyerPostImage entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"UPDATE buyer_posts_images " +
                    $"SET buyer_post_id=@BuyerPostId, image_path=@ImagePath, created_at=@CreatedAt, " +
                        $" updated_at=@UpdatedAt WHERE id={Id};";

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
