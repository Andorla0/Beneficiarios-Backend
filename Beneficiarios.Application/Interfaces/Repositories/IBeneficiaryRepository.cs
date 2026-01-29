using System.Threading;
using System.Threading.Tasks;
using Beneficiarios.Application.Common;
using Beneficiarios.Application.Dtos;
using Beneficiarios.Domain.Entities;

namespace Beneficiarios.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for managing beneficiary entities.
/// Defines operations for create, read, update, and delete (CRUD) operations on beneficiaries.
/// </summary>
public interface IBeneficiaryRepository
{
    /// <summary>
    /// Adds a new beneficiary to the repository asynchronously.
    /// </summary>
    /// <param name="beneficiary">The beneficiary entity to add.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The ID of the newly created beneficiary.</returns>
    Task<int> AddAsync(Beneficiary beneficiary, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing beneficiary in the repository asynchronously.
    /// </summary>
    /// <param name="beneficiary">The beneficiary entity with updated information.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    Task UpdateAsync(Beneficiary beneficiary, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a beneficiary from the repository by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the beneficiary to delete.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a beneficiary by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the beneficiary to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The beneficiary entity if found; otherwise, null.</returns>
    Task<Beneficiary?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paginated list of beneficiaries filtered by the specified criteria asynchronously.
    /// </summary>
    /// <param name="filter">The filter criteria containing name, document number, document type, and pagination parameters.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A paginated result containing beneficiaries matching the filter criteria.</returns>
    Task<PagedResult<Beneficiary>> GetAllAsync(BeneficiaryListFilterDto filter, CancellationToken cancellationToken = default);
}
