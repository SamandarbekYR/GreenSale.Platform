using Dapper;
using GreenSale.Application.Utils;
using GreenSale.DataAccess.Interfaces.Categories;
using GreenSale.Domain.Entites.Categories;
using static Dapper.SqlMapper;

namespace GreenSale.DataAccess.Repositories.Categories;

public class CategoryRepository : BaseRepository, ICategoryRepository
{
    public async Task<long> CountAsync()
    {
        try
        {
            await _connection.OpenAsync();
            string query = $"SELECT COUNT(*) FROM categories;";
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

    public async Task<int> CreateAsync(Category entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = "INSERT INTO public.categories(name, created_at, updated_at) " +
               "VALUES (@Name, @CreatedAt, @UpdatedAt) RETURNING id ";

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
            string query = "DELETE FROM categories WHERE id=@ID ;";
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

    public async Task<List<Category>> GetAllAsync(PaginationParams @params)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"SELECT * FROM categories order by id desc " +
                $"offset {@params.GetSkipCount()} limit {@params.PageSize}";

            var result = (await _connection.QueryAsync<Category>(query)).ToList();

            return result;
        }
        catch
        {
            return new List<Category>() { };
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<Category> GetByIdAsync(long Id)
    {
        try
        {
            await _connection.OpenAsync();
            string query = $"SELECT * FROM categories where id=@ID;";
            var result = await _connection.QuerySingleAsync<Category>(query, new { ID = Id });

            return result;
        }
        catch
        {
            return new Category();
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<string> GetCategoryNameAsync(long categoryId)
    {
        try
        {
            await _connection.OpenAsync();
            string query = $"SELECT name FROM categories where id=@ID;";
            var result = await _connection.QuerySingleAsync<string>(query, new { ID = categoryId });

            return result;
        }
        catch
        {
            return "";
        }
        finally
        {
            await _connection.CloseAsync();
        }
    }

    public async Task<int> UpdateAsync(long Id, Category entity)
    {
        try
        {
            await _connection.OpenAsync();

            string query = $"UPDATE public.categories " +
                $"SET name=@Name, created_at=@CreatedAt, updated_at=@UpdatedAt " +
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
