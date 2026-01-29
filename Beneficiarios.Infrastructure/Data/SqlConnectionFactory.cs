using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Beneficiarios.Infrastructure.Data;

/// <summary>
/// Factory interface for creating and opening SQL database connections.
/// </summary>
public interface ISqlConnectionFactory
{
    /// <summary>
    /// Creates and opens a new SQL connection asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>An open SQL connection ready for use.</returns>
    Task<SqlConnection> CreateOpenConnectionAsync(
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Factory for creating and opening SQL Server database connections.
/// Encapsulates the connection string and handles connection lifecycle.
/// </summary>
public sealed class SqlConnectionFactory : ISqlConnectionFactory
{
    // Dependencies
    private readonly string _connectionString;

    /// <summary>
    /// Constructor that initializes the factory with a connection string.
    /// </summary>
    /// <param name="connectionString">The SQL Server connection string. Must not be null or empty.</param>
    /// <exception cref="ArgumentNullException">Thrown when connection string is null.</exception>
    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString
            ?? throw new ArgumentNullException(nameof(connectionString));
    }

    /// <summary>
    /// Creates and opens a new SQL connection asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>An open and ready-to-use SQL connection.</returns>
    public async Task<SqlConnection> CreateOpenConnectionAsync(
        CancellationToken cancellationToken = default)
    {
        // Create new connection instance with connection string
        var connection = new SqlConnection(_connectionString);

        // Open the connection asynchronously
        await connection.OpenAsync(cancellationToken);

        // Return the opened connection
        return connection;
    }
}