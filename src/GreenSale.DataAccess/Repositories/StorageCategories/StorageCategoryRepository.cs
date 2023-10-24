using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.StorageCategories;
using GreenSale.Domain.Entites.Storages;

namespace GreenSale.DataAccess.Repositories.StorageCategories
{
    public class StorageCategoryRepository : BaseRepository, IStorageCategoryRepository
    {
        public async Task<long> CountAsync()
        {
            try
            {
                await _connection.OpenAsync();

                string query = "SELECT COUNT(*) FROM sellerposts;";
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

        public async Task<int> CreateAsync(StorageCategory entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "INSERT INTO public.storage_categories(category_id, storage_id, created_at, updated_at)" +
                    " VALUES (@CategoryId, @StorageId, @CreatedAt, @UpdatedAt) RETURNING id ";

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

                string query = $"DELETE FROM storage_categories WHERE storage_id={Id} or category_id= {Id} ;";
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

        public async Task<List<StorageCategory>> GetAllAsync(PaginationParams @params)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "SELECT * FROM storage_categories ORDER BY id DESC " +
                    $"OFFSET {@params.GetSkipCount()} LIMIT {@params.PageSize};";

                var result = (await _connection.QueryAsync<StorageCategory>(query)).ToList();

                return result;
            }
            catch
            {
                return new List<StorageCategory>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<StorageCategory> GetByIdAsync(long Id)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "SELECT * FROM storage_categories where id=@ID;";
                var result = await _connection.QuerySingleAsync<StorageCategory>(query, new { ID = Id });

                return result;
            }
            catch
            {
                return new StorageCategory();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<long> GetCategoriesAsync(long storageId)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "SELECT category_id FROM storage_categories WHERE storage_id=@ID; ";

                var result = await _connection.QuerySingleAsync<long>(query, new {ID=storageId});

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

        public async Task<List<long>> GetStorageIdAsync(long categoryId)
        {
            try
            {
                await _connection.OpenAsync();

                string query = "SELECT storage_id FROM storage_categories WHERE category_id=@ID; ";

                var result = (await _connection.QueryAsync<long>(query,new {ID=categoryId})).ToList();

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

        public async Task<int> UpdateAsync(long Id, StorageCategory entity)
        {
            try
            {
                await _connection.OpenAsync();

                string query = $"UPDATE public.storage_categories SET category_id = @CategoryId, " +
                    $" storage_id = @StorageId " +
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
}
