using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.SellerPosts;
using GreenSale.Domain.Entites.SellerPosts;

namespace GreenSale.DataAccess.Repositories.SellerPosts;

public class SellerPostImageRepository : BaseRepository, ISellerPostImageRepository
{
    public async Task<long> CountAsync()
    {
        try
        {
            await _connection.OpenAsync();
            string query = "select count(*) from seller_posts_images";
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

    public async Task<int> CreateAsync(SellerPostImage entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "Insert into seller_posts_images(seller_post_id, image_path, created_at, updated_at) " +
                "values ( @SellerPostId, @ImagePath, @CreatedAt, @UpdatedAt) RETURNING id  ";

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
            string query = "Delete from seller_posts_images where seller_post_id = @ID or id = @ID";
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

    public async Task<List<SellerPostImage>> GetAllAsync(PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string qauery = "SELECT * FROM seller_posts_images order by id desc " +
                $" offset {@params.GetSkipCount()} limit {@params.PageSize} ";

            var result = (await _connection.QueryAsync<SellerPostImage>(qauery)).ToList();

            return result;
        }
        catch
        {
            return new List<SellerPostImage>();
        }
        finally
        {
            await _connection.CloseAsync();
        }

    }

    public async Task<List<SellerPostImage>> GetByIdAllAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "select * from seller_posts_images  where seller_post_id = @ID";
            var result = (await _connection.QueryAsync<SellerPostImage>(query, new { ID = Id })).ToList();

            return result;
        }
        catch
        {
            return new List<SellerPostImage>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<SellerPostImage> GetByIdAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();
            string query = "select * from seller_posts_images  where id = @ID";
            var result = await _connection.QuerySingleAsync<SellerPostImage>(query, new { Id = Id });

            return result;
        }
        catch
        {
            return new SellerPostImage();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<SellerPostImage>> GetFirstAllAsync()
    {
        try
        {
            await _connection.OpenAsync();

            string qauery = "SELECT id, seller_post_id, image_path, created_at, updated_at " +
                "FROM public.seller_posts_images WHERE (seller_post_id, id) " +
                    "IN (SELECT seller_post_id, MIN(id) FROM public.seller_posts_images " +
                        "GROUP BY seller_post_id ) ORDER BY id DESC";

            var result = (await _connection.QueryAsync<SellerPostImage>(qauery)).ToList();

            return result;
        }
        catch
        {
            return new List<SellerPostImage>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> UpdateAsync(long Id, SellerPostImage entity)
    {

        try
        {
            await _connection.OpenAsync();

            string query = $"UPDATE seller_posts_images " +
                $"SET seller_post_id=@SellerPostId, image_path=@ImagePath, created_at=@CreatedAt, " +
                    $" updated_at=@UpdatedAt WHERE id={Id} RETURNING id ";

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
