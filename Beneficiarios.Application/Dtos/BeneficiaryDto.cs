using System;

namespace Beneficiarios.Application.Dtos;

/// <summary>
/// Data transfer object for beneficiary information.
/// Used to transfer beneficiary data between application layers.
/// </summary>
public class BeneficiaryDto
{
    /// <summary>
    /// Unique identifier of the beneficiary.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// First names of the beneficiary.
    /// </summary>
    public string FirstNames { get; init; } = string.Empty;

    /// <summary>
    /// Last names of the beneficiary.
    /// </summary>
    public string LastNames { get; init; } = string.Empty;

    /// <summary>
    /// Identifier of the identity document type.
    /// </summary>
    public int IdentityDocumentId { get; init; }

    /// <summary>
    /// Identity document number.
    /// </summary>
    public string DocumentNumber { get; init; } = string.Empty;

    /// <summary>
    /// Date of birth of the beneficiary.
    /// </summary>
    public DateOnly BirthDate { get; init; }

    /// <summary>
    /// Gender of the beneficiary (M or F).
    /// </summary>
    public string Gender { get; init; } = string.Empty;
}
