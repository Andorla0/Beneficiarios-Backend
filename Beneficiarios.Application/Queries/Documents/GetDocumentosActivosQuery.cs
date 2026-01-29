using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Beneficiarios.Application.Dtos;
using Beneficiarios.Application.Interfaces.Repositories;
using Beneficiarios.Application.Mappers;

namespace Beneficiarios.Application.Queries.Documentos;

/// <summary>
/// Query to retrieve all active identity documents.
/// </summary>
public class GetActiveIdentityDocumentsQuery
{
}

/// <summary>
/// Handler for retrieving all active identity documents.
/// Fetches active identity documents from the repository and returns them as DTOs.
/// </summary>
public class GetActiveIdentityDocumentsQueryHandler
{
    // Dependencies
    private readonly IIdentityDocumentRepository _documentRepository;

    /// <summary>
    /// Constructor that initializes the handler with the required repository.
    /// </summary>
    public GetActiveIdentityDocumentsQueryHandler(IIdentityDocumentRepository documentRepository)
    {
        _documentRepository = documentRepository;
    }

    /// <summary>
    /// Executes the query to retrieve all active identity documents.
    /// </summary>
    /// <param name="query">The get active identity documents query.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A read-only collection of active identity document data transfer objects.</returns>
    public async Task<IReadOnlyCollection<IdentityDocumentDto>> HandleAsync(GetActiveIdentityDocumentsQuery query, CancellationToken cancellationToken = default)
    {
        // Retrieve active identity documents from repository
        var documents = await _documentRepository.GetActiveAsync(cancellationToken);
        
        // Map entities to DTOs and return as read-only collection
        return documents.Select(IdentityDocumentMapper.ToDto).ToArray();
    }
}
