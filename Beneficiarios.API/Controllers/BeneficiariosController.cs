using Beneficiarios.Application.Commands.Beneficiarios;
using Beneficiarios.Application.Dtos;
using Beneficiarios.Application.Queries.Beneficiarios;
using Microsoft.AspNetCore.Mvc;

namespace Beneficiarios.API.Controllers;

/// <summary>
/// API controller for managing beneficiaries.
/// Provides endpoints for creating, reading, updating, and deleting beneficiary records.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class BeneficiariesController : ControllerBase
{
    // Command and query handlers for business logic operations
    private readonly CreateBeneficiaryCommandHandler _createHandler;
    private readonly UpdateBeneficiaryCommandHandler _updateHandler;
    private readonly DeleteBeneficiaryCommandHandler _deleteHandler;
    private readonly GetBeneficiaryByIdQueryHandler _getByIdHandler;
    private readonly GetBeneficiariesQueryHandler _listHandler;

    public BeneficiariesController(
        CreateBeneficiaryCommandHandler createHandler,
        UpdateBeneficiaryCommandHandler updateHandler,
        DeleteBeneficiaryCommandHandler deleteHandler,
        GetBeneficiaryByIdQueryHandler getByIdHandler,
        GetBeneficiariesQueryHandler listHandler)
    {
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
        _getByIdHandler = getByIdHandler;
        _listHandler = listHandler;
    }

    /// <summary>
    /// Retrieves paginated beneficiaries with optional filters.
    /// </summary>
    /// <param name="filter">Optional filter criteria for listing beneficiaries.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A paginated list of beneficiary records.</returns>
    [HttpGet]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] BeneficiaryListFilterDto filter, CancellationToken cancellationToken)
    {
        var result = await _listHandler.HandleAsync(new GetBeneficiariesQuery { Filter = filter }, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a specific beneficiary by their identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the beneficiary.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The beneficiary data transfer object if found; otherwise, a 404 Not Found response.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(BeneficiaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var dto = await _getByIdHandler.HandleAsync(new GetBeneficiaryByIdQuery { Id = id }, cancellationToken);
        return dto is null ? NotFound() : Ok(dto);
    }

    /// <summary>
    /// Creates a new beneficiary record.
    /// </summary>
    /// <param name="command">The command containing the beneficiary data to create.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The newly created beneficiary data transfer object with HTTP 201 Created status.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(BeneficiaryDto), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateBeneficiaryCommand command, CancellationToken cancellationToken)
    {
        var dto = await _createHandler.HandleAsync(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    /// <summary>
    /// Updates an existing beneficiary record.
    /// </summary>
    /// <param name="id">The unique identifier of the beneficiary to update.</param>
    /// <param name="command">The command containing the updated beneficiary data.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>The updated beneficiary data transfer object.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(BeneficiaryDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBeneficiaryCommand command, CancellationToken cancellationToken)
    {
        // Create an update command with the route ID to ensure consistency
        var fixedCommand = CreateUpdateCommand(id, command);
        var dto = await _updateHandler.HandleAsync(fixedCommand, cancellationToken);
        return Ok(dto);
    }

    /// <summary>
    /// Helper method to create an update command with the route identifier.
    /// Ensures the ID from the route takes precedence over any ID in the request body.
    /// </summary>
    private static UpdateBeneficiaryCommand CreateUpdateCommand(int id, UpdateBeneficiaryCommand command)
        => new()
        {
            Id = id,
            FirstNames = command.FirstNames,
            LastNames = command.LastNames,
            IdentityDocumentId = command.IdentityDocumentId,
            DocumentNumber = command.DocumentNumber,
            BirthDate = command.BirthDate,
            Gender = command.Gender
        };

    /// <summary>
    /// Deletes a beneficiary record.
    /// </summary>
    /// <param name="id">The unique identifier of the beneficiary to delete.</param>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>HTTP 204 No Content response on successful deletion.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _deleteHandler.HandleAsync(new DeleteBeneficiaryCommand { Id = id }, cancellationToken);
        return NoContent();
    }
}
