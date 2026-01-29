using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Beneficiarios.Application.Interfaces.Repositories;
using Beneficiarios.Domain.Entities;
using Beneficiarios.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace Beneficiarios.Infrastructure.Repositories;

/// <summary>
/// Repository for managing identity document data in SQL Server.
/// Implements queries for retrieving identity document information using stored procedures.
/// </summary>
public sealed class IdentityDocumentRepository : IIdentityDocumentRepository
{
    // Dependencies
    private readonly ISqlConnectionFactory _connectionFactory;

    /// <summary>
    /// Constructor that initializes the repository with a SQL connection factory.
    /// </summary>
    public IdentityDocumentRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Retrieves an identity document by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the identity document to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The identity document entity if found; otherwise, null.</returns>
    public async Task<IdentityDocument?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // Create and open database connection
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        
        // Create stored procedure command
        await using var cmd = new SqlCommand("dbo.IdentityDocument_GetById", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        // Add parameter
        cmd.Parameters.AddWithValue("@Id", id);

        // Execute and read results
        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        if (!await reader.ReadAsync(cancellationToken))
        {
            return null;
        }

        // Map and return the identity document entity
        return MapIdentityDocument(reader);
    }

    /// <summary>
    /// Retrieves all active identity documents asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A read-only collection of all active identity document entities.</returns>
    public async Task<IReadOnlyCollection<IdentityDocument>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        // Create and open database connection
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        
        // Create stored procedure command
        await using var cmd = new SqlCommand("dbo.IdentityDocument_GetActive", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        // Read results into list
        var items = new List<IdentityDocument>();

        await using var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            items.Add(MapIdentityDocument(reader));
        }

        // Return read-only collection
        return items;
    }

    /// <summary>
    /// Maps a SQL data reader row to an identity document entity.
    /// </summary>
    /// <param name="reader">The SQL data reader positioned at the current row.</param>
    /// <returns>An identity document entity with all properties populated.</returns>
    private static IdentityDocument MapIdentityDocument(SqlDataReader reader)
    {
        // Map and return identity document entity from reader columns
        return new IdentityDocument(
            id: reader.GetInt32(reader.GetOrdinal("Id")),
            name: reader.GetString(reader.GetOrdinal("Name")),
            abbreviation: reader.GetString(reader.GetOrdinal("Abbreviation")),
            country: reader.GetString(reader.GetOrdinal("Country")),
            length: reader.GetInt32(reader.GetOrdinal("Length")),
            numericOnly: reader.GetBoolean(reader.GetOrdinal("NumericOnly")),
            isActive: reader.GetBoolean(reader.GetOrdinal("IsActive"))
        );
    }
}
