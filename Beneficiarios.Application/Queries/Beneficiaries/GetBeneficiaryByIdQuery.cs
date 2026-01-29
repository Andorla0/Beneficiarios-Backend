using System.Threading;
using System.Threading.Tasks;
using Beneficiarios.Application.Dtos;
using Beneficiarios.Application.Interfaces.Repositories;
using Beneficiarios.Application.Mappers;

namespace Beneficiarios.Application.Queries.Beneficiarios;

/// <summary>
/// Query to retrieve a beneficiary by its identifier.
/// </summary>
public class GetBeneficiaryByIdQuery
{
    /// <summary>
    /// Identifier of the beneficiary to retrieve.
    /// </summary>
    public int Id { get; init; }
}

/// <summary>
/// Handler for retrieving a beneficiary by its identifier.
/// Fetches the beneficiary from the repository and returns it as a DTO.
/// </summary>
public class GetBeneficiaryByIdQueryHandler
{
    // Dependencies
    private readonly IBeneficiaryRepository _beneficiaryRepository;

    /// <summary>
    /// Constructor that initializes the handler with the required repository.
    /// </summary>
    public GetBeneficiaryByIdQueryHandler(IBeneficiaryRepository beneficiaryRepository)
    {
        _beneficiaryRepository = beneficiaryRepository;
    }

    /// <summary>
    /// Executes the query to retrieve a beneficiary by its ID.
    /// </summary>
    /// <param name="query">The get beneficiary by ID query containing the ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>Data transfer object of the beneficiary if found; otherwise, null.</returns>
    public async Task<BeneficiaryDto?> HandleAsync(GetBeneficiaryByIdQuery query, CancellationToken cancellationToken = default)
    {
        // Retrieve beneficiary entity from repository
        var entity = await _beneficiaryRepository.GetByIdAsync(query.Id, cancellationToken);

        // Map to DTO if entity exists, otherwise return null
        return entity is null ? null : BeneficiaryMapper.ToDto(entity);
    }
}
