namespace Beneficiarios.Application.Dtos;

/// <summary>
/// Data transfer object for filtering beneficiary list queries.
/// Provides filtering and pagination parameters for retrieving beneficiaries.
/// </summary>
public class BeneficiaryListFilterDto
{
    /// <summary>
    /// Optional filter by beneficiary name (first or last name).
    /// </summary>
    public string? Name { get; init; }

    /// <summary>
    /// Optional filter by identity document number.
    /// </summary>
    public string? DocumentNumber { get; init; }

    /// <summary>
    /// Optional filter by identity document type identifier.
    /// </summary>
    public int? IdentityDocumentId { get; init; }

    /// <summary>
    /// Current page number for pagination (1-based index). Default is 1.
    /// </summary>
    public int Page { get; init; } = 1;

    /// <summary>
    /// Number of items per page. Default is 20.
    /// </summary>
    public int PageSize { get; init; } = 20;
}
