using Beneficiarios.Application.Queries.Documentos;
using Microsoft.AspNetCore.Mvc;

namespace Beneficiarios.API.Controllers;

/// <summary>
/// API controller for managing identity document types.
/// Provides endpoints for retrieving available identity document information.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class DocumentsController : ControllerBase
{
    // Query handler for retrieving active identity documents
    private readonly GetActiveIdentityDocumentsQueryHandler _handler;

    public DocumentsController(GetActiveIdentityDocumentsQueryHandler handler)
    {
        _handler = handler;
    }

    /// <summary>
    /// Retrieves a list of all active identity document types.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the async operation.</param>
    /// <returns>A list of active identity document types.</returns>
    [HttpGet("active")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetActive(CancellationToken cancellationToken)
    {
        // Execute the query to retrieve active documents
        var items = await _handler.HandleAsync(new GetActiveIdentityDocumentsQuery(), cancellationToken);
        return Ok(items);
    }
}
