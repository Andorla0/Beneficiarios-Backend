using Beneficiarios.Domain.Exceptions;

namespace Beneficiarios.Domain.Entities;

/// <summary>
/// Domain entity that represents an identity document type.
/// </summary>
public class IdentityDocument
{
    // Validation messages
    private const string NameRequiredMsg = "Document name is required.";
    private const string AbbreviationRequiredMsg = "Document abbreviation is required.";
    private const string CountryRequiredMsg = "Document country is required.";
    private const string InvalidLengthMsg = "Document length must be greater than zero.";

    /// <summary>
    /// Unique identifier of the identity document.
    /// </summary>
    public int Id { get; }

    /// <summary>
    /// Descriptive name of the document (e.g., Identity Card).
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Abbreviation of the document (e.g., ID).
    /// </summary>
    public string Abbreviation { get; }

    /// <summary>
    /// Country of issue of the document.
    /// </summary>
    public string Country { get; }

    /// <summary>
    /// Expected length of the document number.
    /// </summary>
    public int Length { get; }

    /// <summary>
    /// Indicates whether the document only accepts numeric values.
    /// </summary>
    public bool NumericOnly { get; }

    /// <summary>
    /// Active/inactive status of the document.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Constructor that initializes an identity document with validations.
    /// </summary>
    public IdentityDocument(
        int id,
        string name,
        string abbreviation,
        string country,
        int length,
        bool numericOnly,
        bool isActive)
    {
        // Validate required fields
        ValidateRequiredField(name, NameRequiredMsg);
        ValidateRequiredField(abbreviation, AbbreviationRequiredMsg);
        ValidateRequiredField(country, CountryRequiredMsg);

        // Validate length
        if (length <= 0)
        {
            throw new DomainValidationException(InvalidLengthMsg);
        }

        // Assign properties
        Id = id;
        Name = name.Trim();
        Abbreviation = abbreviation.Trim();
        Country = country.Trim();
        Length = length;
        NumericOnly = numericOnly;
        IsActive = isActive;
    }

    /// <summary>
    /// Activates the identity document.
    /// </summary>
    public void Activate()
    {
        IsActive = true;
    }

    /// <summary>
    /// Deactivates the identity document.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Validates that a required field is not empty.
    /// </summary>
    private static void ValidateRequiredField(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException(message);
        }
    }
}
