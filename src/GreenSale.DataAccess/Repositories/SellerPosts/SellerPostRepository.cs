using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.SellerPosts;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.Domain.Entites.SelerPosts;

namespace GreenSale.DataAccess.Repositories.SellerPosts;

public class SellerPostRepository : BaseRepository, ISellerPostsRepository
{
    public async Task<long> CountAsync()
    {
        try
        {
            await _connection.OpenAsync();
            string query = "SELECT COUNT(*) FROM seller_posts;";
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

    public async Task<long> CountStatusAgreeAsync()
    {
        try
        {
            await _connection.OpenAsync();
            string query = $"SELECT COUNT(*) FROM seller_posts where status = '1'";
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

    public async Task<long> CountStatusNewAsync()
    {
        try
        {
            await _connection.OpenAsync();
            string query = $"SELECT COUNT(*) FROM seller_posts where status = '0'";
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

    public async Task<int> CreateAsync(SellerPost entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "INSERT INTO public.seller_posts( user_id, title, description, price, capacity, " +
                " capacity_measure, type, region, district, status, category_id, phone_number, created_at, updated_at) " +
                    " VALUES ( @UserId, @Title, @Description, @Price, @Capacity, @CapacityMeasure, @Type, @Region, " +
                        " @District, @Status, @CategoryId, @PhoneNumber, @CreatedAt, @UpdatedAt) RETURNING id ";

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
            string query = "DELETE FROM seller_posts WHERE id=@ID;";
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

    public async Task<List<SellerPostViewModel>> GetAllAsync(PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "SELECT * FROM seller_post_viewmodel ORDER BY id DESC " +
                $" OFFSET {@params.GetSkipCount()} LIMIT {@params.PageSize};";

            var result = (await _connection.QueryAsync<SellerPostViewModel>(query)).ToList();

            return result;
        }
        catch
        {
            return new List<SellerPostViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<SellerPostViewModel>> GetAllByIdAsync(long userId, PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"SELECT * FROM seller_post_viewmodel where userId = {userId} ORDER BY id DESC " +
                   $" OFFSET {@params.GetSkipCount()} LIMIT {@params.PageSize};";

            var result = (await _connection.QueryAsync<SellerPostViewModel>(query)).ToList();

            return result;
        }
        catch
        {
            return new List<SellerPostViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<SellerPostViewModel>> GetAllByIdAsync(long userId)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"SELECT * FROM seller_post_viewmodel where userId = {userId} ORDER BY id DESC ";

            var result = (await _connection.QueryAsync<SellerPostViewModel>(query)).ToList();

            return result;
        }
        catch
        {
            return new List<SellerPostViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<SellerPost>> GetAllByIdSellerAsync(long CaregotyId)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"SELECT * FROM seller_posts WHERE category_id ={CaregotyId} ORDER BY id DESC ";

            var result = (await _connection.QueryAsync<SellerPost>(query)).ToList();

            return result;
        }
        catch
        {
            return new List<SellerPost>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<SellerPostViewModel> GetByIdAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();
            string query = "SELECT * FROM seller_post_viewmodel where id=@ID;";
            var result = await _connection.QuerySingleAsync<SellerPostViewModel>(query, new { ID = Id });

            return result;
        }
        catch
        {
            return new SellerPostViewModel();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<SellerPost> GetIdAsync(long postId)
    {
        try
        {
            await _connection.OpenAsync();
            string query = "SELECT * FROM seller_posts where id=@ID;";
            var result = await _connection.QuerySingleAsync<SellerPost>(query, new { ID = postId });

            return result;
        }
        catch
        {
            return new SellerPost();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<(int ItemsCount, List<SellerPostViewModel>)> SearchAsync(string search)
    {
        try
        {
            await _connection.OpenAsync();

            string query = @$" SELECT *  FROM seller_post_viewmodel  WHERE  title ILIKE '%{search}%' order by id desc ;";

            /* var parameters = new
             {
                 Search = search,
                 offset = @params.PageNumber * @params.PageSize,
                 limit = @params.PageSize
             };
 */
            var result = await _connection.QueryAsync<SellerPostViewModel>(query);
            int Count = result.Count();

            return (Count, result.ToList());
        }
        catch
        {
            return (0, new List<SellerPostViewModel>());
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<PostCreatedAt>> SellerDaylilyCreatedAsync(string day)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "SELECT DATE(created_at) AS kun, COUNT(*) FROM seller_posts " +
                $"WHERE DATE(created_at) >= CURRENT_DATE - INTERVAL '{day} days' GROUP BY kun ORDER BY kun;";

            var result = (await _connection.QueryAsync<PostCreatedAt>(query)).ToList();

            return result;
        }
        catch
        {
            return new List<PostCreatedAt>();
        }
    }

    public async Task<List<PostCreatedAt>> SellerMonthlyCreatedAsync(string month)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "SELECT DATE_TRUNC('month', created_at) AS oy, COUNT(*) FROM seller_posts " +
                $"WHERE created_at >= CURRENT_DATE - INTERVAL '{month} months' GROUP BY oy ORDER BY oy;";

            var result = (await _connection.QueryAsync<PostCreatedAt>(query)).ToList();

            return result;
        }
        catch
        {
            return new List<PostCreatedAt>();
        }
    }

    public async Task<int> UpdateAsync(long Id, SellerPost entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"UPDATE public.seller_posts " +
                $"SET  user_id = @UserId, title = @Title, description = @Description, price = @Price, capacity = @Capacity, " +
                    $" capacity_measure = @CapacityMeasure, type = @Type, region = @Region, district = @District, " +
                        $" status = @Status,  phone_number = @PhoneNumber, created_at = @CreatedAt, " +
                            $" updated_at = @UpdatedAt WHERE id={Id} RETURNING id ";

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