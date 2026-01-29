namespace Beneficiarios.Application.Dtos;

/// <summary>
/// Data transfer object for identity document information.
/// Used to transfer identity document data between application layers.
/// </summary>
public class IdentityDocumentDto
{
    /// <summary>
    /// Unique identifier of the identity document type.
    /// </summary>
    public int Id { get; init; }

    /// <summary>
    /// Descriptive name of the document (e.g., Identity Card).
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Abbreviation of the document (e.g., ID).
    /// </summary>
    public string Abbreviation { get; init; } = string.Empty;

    /// <summary>
    /// Country of issue of the document.
    /// </summary>
    public string Country { get; init; } = string.Empty;

    /// <summary>
    /// Expected length of the document number.
    /// </summary>
    public int Length { get; init; }

    /// <summary>
    /// Indicates whether the document only accepts numeric values.
    /// </summary>
    public bool NumericOnly { get; init; }

    /// <summary>
    /// Active/inactive status of the document.
    /// </summary>
    public bool IsActive { get; init; }
}
