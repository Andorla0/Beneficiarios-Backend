using System;
using System.Threading;
using System.Threading.Tasks;
using Beneficiarios.Application.Dtos;
using Beneficiarios.Application.Interfaces.Repositories;
using Beneficiarios.Application.Mappers;
using Beneficiarios.Domain.Entities;
using Beneficiarios.Domain.Exceptions;

namespace Beneficiarios.Application.Commands.Beneficiarios;

/// <summary>
/// Command to create a new beneficiary with basic information.
/// </summary>
public class CreateBeneficiaryCommand
{
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
/// Handler for creating a new beneficiary.
/// Validates the identity document exists and creates the beneficiary in the repository.
/// </summary>
public class CreateBeneficiaryCommandHandler
{
    // Validation messages
    private const string DocumentNotFoundMsg = "Identity document not found or inactive.";

    // Dependencies
    private readonly IBeneficiaryRepository _beneficiaryRepository;
    private readonly IIdentityDocumentRepository _documentRepository;

    /// <summary>
    /// Constructor that initializes the handler with required repositories.
    /// </summary>
    public CreateBeneficiaryCommandHandler(
        IBeneficiaryRepository beneficiaryRepository,
        IIdentityDocumentRepository documentRepository)
    {
        _beneficiaryRepository = beneficiaryRepository;
        _documentRepository = documentRepository;
    }

    /// <summary>
    /// Executes the command to create a new beneficiary.
    /// </summary>
    /// <param name="command">The create beneficiary command.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>Data transfer object of the created beneficiary.</returns>
    /// <exception cref="DomainValidationException">Thrown when identity document is not found.</exception>
    public async Task<BeneficiaryDto> HandleAsync(CreateBeneficiaryCommand command, CancellationToken cancellationToken = default)
    {
        // Retrieve and validate identity document
        var document = await _documentRepository.GetByIdAsync(command.IdentityDocumentId, cancellationToken);
        if (document is null)
        {
            throw new DomainValidationException(DocumentNotFoundMsg);
        }

        // Create new beneficiary entity
        var beneficiary = new Beneficiary(
            id: 0,
            firstNames: command.FirstNames,
            lastNames: command.LastNames,
            document: document,
            documentNumber: command.DocumentNumber,
            birthDate: command.BirthDate,
            gender: command.Gender);

        // Persist beneficiary and assign generated ID
        var newId = await _beneficiaryRepository.AddAsync(beneficiary, cancellationToken);
        beneficiary.SetId(newId);

        // Return mapped DTO
        return BeneficiaryMapper.ToDto(beneficiary);
    }
}
