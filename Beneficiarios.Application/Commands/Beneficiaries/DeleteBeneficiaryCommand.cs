using System.Threading;
using System.Threading.Tasks;
using Beneficiarios.Application.Interfaces.Repositories;
using Beneficiarios.Domain.Exceptions;

namespace Beneficiarios.Application.Commands.Beneficiarios;

/// <summary>
/// Command to delete an existing beneficiary by its identifier.
/// </summary>
public class DeleteBeneficiaryCommand
{
    /// <summary>
    /// Identifier of the beneficiary to delete.
    /// </summary>
    public int Id { get; init; }
}

/// <summary>
/// Handler for deleting a beneficiary.
/// Validates the beneficiary identifier and removes it from the repository.
/// </summary>
public class DeleteBeneficiaryCommandHandler
{
    // Validation messages
    private const string InvalidIdMsg = "Beneficiary ID is required and must be greater than zero.";

    // Dependencies
    private readonly IBeneficiaryRepository _beneficiaryRepository;

    /// <summary>
    /// Constructor that initializes the handler with the required repository.
    /// </summary>
    public DeleteBeneficiaryCommandHandler(IBeneficiaryRepository beneficiaryRepository)
    {
        _beneficiaryRepository = beneficiaryRepository;
    }

    /// <summary>
    /// Executes the command to delete a beneficiary.
    /// </summary>
    /// <param name="command">The delete beneficiary command containing the ID.</param>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <exception cref="DomainValidationException">Thrown when the beneficiary ID is invalid.</exception>
    public async Task HandleAsync(DeleteBeneficiaryCommand command, CancellationToken cancellationToken = default)
    {
        // Validate beneficiary ID
        if (command.Id <= 0)
        {
            throw new DomainValidationException(InvalidIdMsg);
        }

        // Delete beneficiary from repository
        await _beneficiaryRepository.DeleteAsync(command.Id, cancellationToken);
    }
}
