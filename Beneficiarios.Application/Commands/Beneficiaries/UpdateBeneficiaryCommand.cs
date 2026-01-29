using System;
using System.Threading;
using System.Threading.Tasks;
using Beneficiarios.Application.Dtos;
using Beneficiarios.Application.Interfaces.Repositories;
using Beneficiarios.Application.Mappers;
using Beneficiarios.Domain.Exceptions;

namespace Beneficiarios.Application.Commands.Beneficiarios;

/// <summary>
/// Command to update an existing beneficiary's information.
/// </summary>
public class UpdateBeneficiaryCommand
{
    /// <summary>
    /// Identifier of the beneficiary to update.
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
    /// Identity document type identifier.
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

/// <summary>
/// Handler for updating an existing beneficiary.
/// Validates the beneficiary and identity document exist, then updates the beneficiary information.
/// </summary>
public class UpdateBeneficiaryCommandHandler
{
    // Validation messages
    private const string InvalidIdMsg = "Beneficiary ID is required and must be greater than zero.";
    private const string BeneficiaryNotFoundMsg = "Beneficiary not found.";
    private const string DocumentNotFoundMsg = "Identity document not found or inactive.";

    // Dependencies
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly IIdentityDocumentRepository _documentRepository;

    /// <summary>
    /// Constructor that initializes the handler with required repositories.
    /// </summary>
    public UpdateBeneficiaryCommandHandler(
        IBeneficiaryRepository beneficiaryRepository,
        IIdentityDocumentRepository documentRepository)
    {
        _beneficiaryRepository = beneficiaryRepository;
        _documentRepository = documentRepository;
    }

    /// <summary>
    /// Executes the command to update a beneficiary.
    /// </summary>
    /// <param name="command">The update beneficiary command.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>Data transfer object of the updated beneficiary.</returns>
    /// <exception cref="DomainValidationException">Thrown when ID is invalid, beneficiary or identity document not found.</exception>
    public async Task<BeneficiaryDto> HandleAsync(UpdateBeneficiaryCommand command, CancellationToken cancellationToken = default)
    {
        // Validate beneficiary ID
        if (command.Id <= 0)
        {
            throw new DomainValidationException(InvalidIdMsg);
        }

        // Retrieve existing beneficiary
        var existing = await _beneficiaryRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existing is null)
        {
            throw new DomainValidationException(BeneficiaryNotFoundMsg);
        }

        // Retrieve and validate identity document
        var document = await _documentRepository.GetByIdAsync(command.IdentityDocumentId, cancellationToken);
        if (document is null)
        {
            throw new DomainValidationException(DocumentNotFoundMsg);
        }

        // Apply domain rules to update beneficiary
        existing.UpdateBasicData(command.FirstNames, command.LastNames, command.BirthDate, command.Gender);
        existing.AssignDocument(document, command.DocumentNumber);

        // Persist updated beneficiary
        await _beneficiaryRepository.UpdateAsync(existing, cancellationToken);

        // Return mapped DTO
        return BeneficiaryMapper.ToDto(existing);
    }
}
