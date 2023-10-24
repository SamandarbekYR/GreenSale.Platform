using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Storages;
using GreenSale.DataAccess.ViewModels.SellerPosts;
using GreenSale.DataAccess.ViewModels.Storages;
using GreenSale.Domain.Entites.Storages;
using static Dapper.SqlMapper;

namespace GreenSale.DataAccess.Repositories.Storages;

public class StorageRepository : BaseRepository, IStorageRepository
{
    public async Task<long> CountAsync()
    {
        try
        {
            await _connection.OpenAsync();
            string query = $"SELECT COUNT(*) FROM storages;";
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

    public async Task<int> CreateAsync(Storage entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "INSERT INTO public.storages(name, description, region, district, address, " +
                " address_latitude, address_longitude, info, image_path, user_id, created_at, updated_at) " +
                    " VALUES (@Name, @Description, @Region, @District, @Address, @AddressLatitude, " +
                        " @AddressLongitude, @Info, @ImagePath, @UserId, @CreatedAt, @UpdatedAt) RETURNING id ";

            var result = await _connection.QuerySingleAsync<int>(query, entity);

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

            string query = "DELETE FROM storages WHERE id=@ID ;";
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

    public async Task<List<StoragesViewModel>> GetAllAsync(PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "SELECT * FROM storage_viewmodel ORDER BY id DESC " +
                $" OFFSET {@params.GetSkipCount()} LIMIT {@params.PageSize};";

            return (await _connection.QueryAsync<StoragesViewModel>(query)).ToList();
        }
        catch
        {
            return new List<StoragesViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<StoragesViewModel>> GetAllByIdAsync(long userId, PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"SELECT * FROM storage_viewmodel where userId = {userId} ORDER BY id DESC " +
                $" OFFSET {@params.GetSkipCount()} LIMIT {@params.PageSize};";

            return (await _connection.QueryAsync<StoragesViewModel>(query)).ToList();
        }
        catch
        {
            return new List<StoragesViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<StoragesViewModel>> GetAllByIdAsync(long userId)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"SELECT * FROM storage_viewmodel where userId = {userId} ORDER BY id DESC ";

            return (await _connection.QueryAsync<StoragesViewModel>(query)).ToList();
        }
        catch
        {
            return new List<StoragesViewModel>();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<StoragesViewModel> GetByIdAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();
            string query = $"SELECT * FROM storage_viewmodel WHERE id={Id};";

            return await _connection.QuerySingleAsync<StoragesViewModel>(query);

        }
        catch
        {
            return new StoragesViewModel();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<(int ItemsCount, List<StoragesViewModel>)> SearchAsync(string search)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $@"SELECT * FROM storage_viewmodel WHERE info ILIKE '%{search}%' order by id desc ;";

            /* var parameters = new
             {
                 search,
                 offset = @params.PageNumber * @params.PageSize,
                 limit = @params.PageSize
             };
 */
            var result = (await _connection.QueryAsync<StoragesViewModel>(query)).ToList();

            return (result.Count(), result);
        }
        catch
        {
            return (0, new List<StoragesViewModel>());
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<List<PostCreatedAt>> StorageDaylilyCreatedAsync(string day)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "SELECT DATE(created_at) AS kun, COUNT(*) FROM storages " +
                $"WHERE DATE(created_at) >= CURRENT_DATE - INTERVAL '{day} days' GROUP BY kun ORDER BY kun;";

            var result = (await _connection.QueryAsync<PostCreatedAt>(query)).ToList();

            return result;
        }
        catch
        {
            return new List<PostCreatedAt>();
        }
    }

    public async Task<List<PostCreatedAt>> StorageMonthlyCreatedAsync(string month)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "SELECT DATE_TRUNC('month', created_at) AS oy, COUNT(*) FROM storages " +
                $"WHERE created_at >= CURRENT_DATE - INTERVAL '{month} months' GROUP BY oy ORDER BY oy;";

            var result = (await _connection.QueryAsync<PostCreatedAt>(query)).ToList();

            return result;
        }
        catch
        {
            return new List<PostCreatedAt>();
        }
    }

    public async Task<int> UpdateAsync(long Id, Storage entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "UPDATE public.storages " +
                "SET name=@Name, description=@Description, region=@Region, district=@District, address=@Address, " +
                    " address_latitude=@AddressLatitude, address_longitude=@AddressLongitude, info=@Info, " +
                        " user_id=@UserId, created_at=@CreatedAt, updated_at=@UpdatedAt " +
                        $" WHERE id={Id} RETURNING id ";

            return await _connection.ExecuteScalarAsync<int>(query, entity);
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

    public async Task<int> UpdateImageAsync(long Id, string imagePath)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "UPDATE public.storages " +
                $"SET image_path={imagePath} " +
                        $" WHERE id={Id} ; ";

            return await _connection.ExecuteScalarAsync<int>(query);
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