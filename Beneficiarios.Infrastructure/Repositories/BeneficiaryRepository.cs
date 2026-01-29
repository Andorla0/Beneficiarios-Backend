using Beneficiarios.Application.Common;
using Beneficiarios.Application.Dtos;
using Beneficiarios.Application.Interfaces.Repositories;
using Beneficiarios.Domain.Entities;
using Beneficiarios.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace Beneficiarios.Infrastructure.Repositories;

/// <summary>
/// Repository for managing beneficiary data in SQL Server.
/// Implements CRUD operations and pagination queries using stored procedures.
/// </summary>
public sealed class BeneficiaryRepository : IBeneficiaryRepository
{
    // Dependencies
    private readonly ISqlConnectionFactory _connectionFactory;

    /// <summary>
    /// Constructor that initializes the repository with a SQL connection factory.
    /// </summary>
    public BeneficiaryRepository(ISqlConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    /// <summary>
    /// Adds a new beneficiary to the database asynchronously.
    /// </summary>
    /// <param name="beneficiary">The beneficiary entity to add.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The ID of the newly created beneficiary.</returns>
    public async Task<int> AddAsync(Beneficiary beneficiary, CancellationToken cancellationToken = default)
    {
        // Create and open database connection
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        
        // Create stored procedure command
        await using var cmd = new SqlCommand("dbo.Beneficiary_Create", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        // Add input parameters
        cmd.Parameters.AddWithValue("@FirstNames", beneficiary.FirstNames);
        cmd.Parameters.AddWithValue("@LastNames", beneficiary.LastNames);
        cmd.Parameters.AddWithValue("@IdentityDocumentId", beneficiary.IdentityDocumentId);
        cmd.Parameters.AddWithValue("@DocumentNumber", beneficiary.DocumentNumber);

        // Convert DateOnly to DateTime for SQL Server (table uses DATE type)
        cmd.Parameters.AddWithValue("@BirthDate", beneficiary.BirthDate.ToDateTime(TimeOnly.MinValue));

        cmd.Parameters.AddWithValue("@Gender", beneficiary.Gender);

        // Add output parameter to retrieve the generated ID
        // NOTE: The stored procedure must include this OUTPUT parameter
        var outputId = new SqlParameter("@NewId", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };
        cmd.Parameters.Add(outputId);

        // Execute the stored procedure
        await cmd.ExecuteNonQueryAsync(cancellationToken);

        // Return the generated ID
        return (int)(outputId.Value ?? 0);
    }

    /// <summary>
    /// Updates an existing beneficiary in the database asynchronously.
    /// </summary>
    /// <param name="beneficiary">The beneficiary entity with updated information.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    public async Task UpdateAsync(Beneficiary beneficiary, CancellationToken cancellationToken = default)
    {
        // Create and open database connection
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        
        // Create stored procedure command
        await using var cmd = new SqlCommand("dbo.Beneficiary_Update", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        // Add parameters for update operation
        cmd.Parameters.AddWithValue("@Id", beneficiary.Id);
        cmd.Parameters.AddWithValue("@FirstNames", beneficiary.FirstNames);
        cmd.Parameters.AddWithValue("@LastNames", beneficiary.LastNames);
        cmd.Parameters.AddWithValue("@IdentityDocumentId", beneficiary.IdentityDocumentId);
        cmd.Parameters.AddWithValue("@DocumentNumber", beneficiary.DocumentNumber);
        cmd.Parameters.AddWithValue("@BirthDate", beneficiary.BirthDate.ToDateTime(TimeOnly.MinValue));
        cmd.Parameters.AddWithValue("@Gender", beneficiary.Gender);

        // Execute the stored procedure
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <summary>
    /// Deletes a beneficiary from the database by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the beneficiary to delete.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        // Create and open database connection
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        
        // Create stored procedure command
        await using var cmd = new SqlCommand("dbo.Beneficiary_Delete", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        // Add parameter
        cmd.Parameters.AddWithValue("@Id", id);

        // Execute the stored procedure
        await cmd.ExecuteNonQueryAsync(cancellationToken);
    }

    /// <summary>
    /// Retrieves a beneficiary by ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the beneficiary to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The beneficiary entity if found; otherwise, null.</returns>
    public async Task<Beneficiary?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // Create and open database connection
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        
        // Create stored procedure command
        await using var cmd = new SqlCommand("dbo.Beneficiary_GetById", conn)
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

        // Map and return the beneficiary entity
        return MapBeneficiary(reader);
    }

    /// <summary>
    /// Retrieves a paginated list of beneficiaries with optional filtering asynchronously.
    /// </summary>
    /// <param name="filter">Filter criteria containing name, document number, document type, and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A paginated result of beneficiary entities.</returns>
    public async Task<PagedResult<Beneficiary>> GetAllAsync(BeneficiaryListFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Validate and normalize pagination parameters
        var page = filter.Page <= 0 ? 1 : filter.Page;
        var pageSize = filter.PageSize is <= 0 or > 200 ? 20 : filter.PageSize;

        // Create and open database connection
        await using var conn = await _connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        
        // Create stored procedure command
        await using var cmd = new SqlCommand("dbo.Beneficiary_ListPaged", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure
        };

        // Add optional filter parameters (nullable)
        cmd.Parameters.AddNullable("@Name", filter.Name);
        cmd.Parameters.AddNullable("@DocumentNumber", filter.DocumentNumber);
        cmd.Parameters.AddNullable("@IdentityDocumentId", filter.IdentityDocumentId);

        // Add pagination parameters
        cmd.Parameters.AddWithValue("@Page", page);
        cmd.Parameters.AddWithValue("@PageSize", pageSize);

        // Add output parameter for total count
        var outputTotal = new SqlParameter("@TotalCount", System.Data.SqlDbType.Int)
        {
            Direction = System.Data.ParameterDirection.Output
        };
        cmd.Parameters.Add(outputTotal);

        // Read results into list
        var items = new List<Beneficiary>();

        await using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
        {
            while (await reader.ReadAsync(cancellationToken))
            {
                items.Add(MapBeneficiary(reader));
            }
        }

        // Reader is now closed, OUTPUT parameter is properly set
        var total = (int)(outputTotal.Value ?? items.Count);
        
        // Return paginated result
        return new PagedResult<Beneficiary>(items, total, page, pageSize);
    }

    /// <summary>
    /// Maps a SQL data reader row to a beneficiary entity.
    /// </summary>
    /// <param name="reader">The SQL data reader positioned at the current row.</param>
    /// <returns>A beneficiary entity with all properties populated.</returns>
    private static Beneficiary MapBeneficiary(SqlDataReader reader)
    {
        // Map identity document from reader columns
        // NOTE: Column aliases must match exactly with stored procedure output
        var document = new IdentityDocument(
            id: reader.GetInt32(reader.GetOrdinal("Document_Id")),
            name: reader.GetString(reader.GetOrdinal("Document_Name")),
            abbreviation: reader.GetString(reader.GetOrdinal("Document_Abbreviation")),
            country: reader.GetString(reader.GetOrdinal("Document_Country")),
            length: reader.GetInt32(reader.GetOrdinal("Document_Length")),
            numericOnly: reader.GetBoolean(reader.GetOrdinal("Document_NumericOnly")),
            isActive: reader.GetBoolean(reader.GetOrdinal("Document_IsActive"))
        );

        // Convert DateTime to DateOnly for birth date
        var birthDateTime = reader.GetDateTime(reader.GetOrdinal("BirthDate"));
        var birthDateOnly = DateOnly.FromDateTime(birthDateTime);

        // Map and return beneficiary entity
        return new Beneficiary(
            id: reader.GetInt32(reader.GetOrdinal("Id")),
            firstNames: reader.GetString(reader.GetOrdinal("FirstNames")),
            lastNames: reader.GetString(reader.GetOrdinal("LastNames")),
            document: document,
            documentNumber: reader.GetString(reader.GetOrdinal("DocumentNumber")),
            birthDate: birthDateOnly,
            gender: reader.GetString(reader.GetOrdinal("Gender"))
        );
    }
}
