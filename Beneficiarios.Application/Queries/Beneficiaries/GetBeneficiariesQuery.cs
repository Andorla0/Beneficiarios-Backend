using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Beneficiarios.Application.Common;
using Beneficiarios.Application.Dtos;
using Beneficiarios.Application.Interfaces.Repositories;
using Beneficiarios.Application.Mappers;

namespace Beneficiarios.Application.Queries.Beneficiarios;

/// <summary>
/// Query to retrieve a paginated list of beneficiaries with optional filtering.
/// </summary>
public class GetBeneficiariesQuery
{
    /// <summary>
    /// Filter criteria for retrieving beneficiaries (name, document number, document type, pagination).
    /// </summary>
    public BeneficiaryListFilterDto Filter { get; init; } = new();
}

/// <summary>
/// Handler for retrieving a paginated list of beneficiaries.
/// Fetches beneficiaries from the repository based on filter criteria and returns them as DTOs.
/// </summary>
public class GetBeneficiariesQueryHandler
{
    // Dependencies
    private readonly IBeneficiaryRepository _beneficiaryRepository;

    /// <summary>
    /// Constructor that initializes the handler with the required repository.
    /// </summary>
    public GetBeneficiariesQueryHandler(IBeneficiaryRepository beneficiaryRepository)
    {
        _beneficiaryRepository = beneficiaryRepository;
    }

    /// <summary>
    /// Executes the query to retrieve a paginated list of beneficiaries.
    /// </summary>
    /// <param name="query">The get beneficiaries query containing filter criteria.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>A paginated result of beneficiary data transfer objects.</returns>
    public async Task<PagedResult<BeneficiaryDto>> HandleAsync(GetBeneficiariesQuery query, CancellationToken cancellationToken = default)
    {
        // Retrieve paginated beneficiaries from repository
        var page = await _beneficiaryRepository.GetAllAsync(query.Filter, cancellationToken);

        // Map entities to DTOs and return paginated result
        return new PagedResult<BeneficiaryDto>(
            items: page.Items.Select(BeneficiaryMapper.ToDto),
            totalCount: page.TotalCount,
            page: page.Page,
            pageSize: page.PageSize);
    }
}
