using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Beneficiarios.Domain.Entities;

namespace Beneficiarios.Application.Interfaces.Repositories;

/// <summary>
/// Repository interface for managing identity document entities.
/// Defines operations for retrieving identity document information.
/// </summary>
public interface IIdentityDocumentRepository
{
    /// <summary>
    /// Retrieves an identity document by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the identity document to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>The identity document entity if found; otherwise, null.</returns>
    Task<IdentityDocument?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves all active identity documents asynchronously.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A read-only collection of all active identity document entities.</returns>
    Task<IReadOnlyCollection<IdentityDocument>> GetActiveAsync(CancellationToken cancellationToken = default);
}
