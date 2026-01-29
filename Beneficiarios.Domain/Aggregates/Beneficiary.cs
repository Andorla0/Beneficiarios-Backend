using System.Linq;
using Beneficiarios.Domain.Exceptions;

namespace Beneficiarios.Domain.Entities;

/// <summary>
/// Domain entity that represents a beneficiary.
/// </summary>
public class Beneficiary
{
    // Validation constants
    private const string GenderMale = "M";
    private const string GenderFemale = "F";

    /// <summary>
    /// Unique identifier of the beneficiary.
    /// </summary>
    public int Id { get; private set; }

    /// <summary>
    /// First names of the beneficiary.
    /// </summary>
    public string FirstNames { get; private set; }

    /// <summary>
    /// Last names of the beneficiary.
    /// </summary>
    public string LastNames { get; private set; }

    /// <summary>
    /// Identifier of the identity document type.
    /// </summary>
    public int IdentityDocumentId { get; private set; }

    /// <summary>
    /// Identity document number.
    /// </summary>
    public string DocumentNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Date of birth of the beneficiary.
    /// </summary>
    public DateOnly BirthDate { get; private set; }

    /// <summary>
    /// Gender of the beneficiary (M or F).
    /// </summary>
    public string Gender { get; private set; }

    /// <summary>
    /// Constructor that initializes a beneficiary with all basic data.
    /// </summary>
    public Beneficiary(
        int id,
        string firstNames,
        string lastNames,
        IdentityDocument document,
        string documentNumber,
        DateOnly birthDate,
        string gender)
    {
        // Initialize identifier
        Id = id;

        // Validate and assign basic data
        FirstNames = ValidateName(firstNames, "FirstNames");
        LastNames = ValidateName(lastNames, "LastNames");
        BirthDate = birthDate;
        Gender = ValidateGender(gender);

        // Assign identity document
        AssignDocument(document, documentNumber);
    }

    /// <summary>
    /// Updates the basic data of the beneficiary.
    /// </summary>
    public void UpdateBasicData(string firstNames, string lastNames, DateOnly birthDate, string gender)
    {
        FirstNames = ValidateName(firstNames, "FirstNames");
        LastNames = ValidateName(lastNames, "LastNames");
        BirthDate = birthDate;
        Gender = ValidateGender(gender);
    }

    /// <summary>
    /// Assigns an identity document to the beneficiary.
    /// </summary>
    public void AssignDocument(IdentityDocument document, string documentNumber)
    {
        // Validate that document is not null
        if (document is null)
        {
            throw new DomainValidationException("Identity document is required.");
        }

        // Validate that document is active
        if (!document.IsActive)
        {
            throw new DomainValidationException("Identity document must be active.");
        }

        // Validate document number
        ValidateDocumentNumber(documentNumber, document);

        // Assign document and number
        IdentityDocumentId = document.Id;
        DocumentNumber = documentNumber.Trim();
    }

    /// <summary>
    /// Sets the identifier of the beneficiary.
    /// </summary>
    public void SetId(int id)
    {
        if (id <= 0)
        {
            throw new DomainValidationException("Identifier must be greater than zero.");
        }

        Id = id;
    }

    /// <summary>
    /// Validates that a name or last name is not empty.
    /// </summary>
    private static string ValidateName(string value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainValidationException($"{fieldName} is required.");
        }

        return value.Trim();
    }

    /// <summary>
    /// Validates that gender is 'M' or 'F'.
    /// </summary>
    private static string ValidateGender(string gender)
    {
        if (string.IsNullOrWhiteSpace(gender))
        {
            throw new DomainValidationException("Gender is required.");
        }

        var normalized = gender.Trim().ToUpperInvariant();

        // Validate allowed values
        if (normalized is not ($"{GenderMale}" or $"{GenderFemale}"))
        {
            throw new DomainValidationException($"Gender must be '{GenderMale}' or '{GenderFemale}'.");
        }

        return normalized;
    }

    /// <summary>
    /// Validates the document number according to the document type rules.
    /// </summary>
    private static void ValidateDocumentNumber(string documentNumber, IdentityDocument document)
    {
        // Validate that number is not empty
        if (string.IsNullOrWhiteSpace(documentNumber))
        {
            throw new DomainValidationException("Document number is required.");
        }

        var clean = documentNumber.Trim();

        // Validate length
        if (clean.Length != document.Length)
        {
            throw new DomainValidationException($"Document number must have {document.Length} characters.");
        }

        // Validate that it only contains digits if required
        if (document.NumericOnly && clean.Any(c => !char.IsDigit(c)))
        {
            throw new DomainValidationException("Document number only accepts digits.");
        }
    }
}
