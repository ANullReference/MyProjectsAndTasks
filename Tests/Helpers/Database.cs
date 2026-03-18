using Infrastructure.DatabaseRepository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.Helpers;

public class Database : IAsyncDisposable
{
    private SqliteConnection _connection;

    public Database()
    {
        _connection = new("Filename=:memory:");
    }

    public ApplicationDbContext Generate()
    {
        if (_connection is null)
        {
            InvalidProgramException ex = new("Connection is null. This should never happen.");
            throw ex;
        }

        // 1. Create and open the connection
        _connection.Open();

        // 2. Build options and ensure the schema is created
        DbContextOptions<ApplicationDbContext> options = new DbContextOptionsBuilder<ApplicationDbContext>()
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