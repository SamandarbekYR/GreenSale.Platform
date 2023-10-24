using Npgsql;

namespace GreenSale.DataAccess.Repositories;

public class BaseRepository
{
    protected NpgsqlConnection _connection;

    public BaseRepository()
    {
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;


        
        string connection = "Host=greensale-database-host; Port=5432; User Id=postgres_admin; Password=AAaa@@11; Database=greensale-db;";
        this._connection = new NpgsqlConnection(connection);
    }
}