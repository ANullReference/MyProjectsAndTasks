using Infrastructure.DatabaseRepository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.Helpers;

public class Database : IAsyncDisposable
{
    private SqliteConnection _connection;

    public ApplicationDbContext Generate()
    {
        // 1. Create and open the connection
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        // 2. Build options and ensure the schema is created
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .Options;

        return new ApplicationDbContext(options);
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
        await _connection.CloseAsync();
    }
}