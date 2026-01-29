using Beneficiarios.Application.Dtos;
using Beneficiarios.Domain.Entities;

namespace Beneficiarios.Application.Mappers;

/// <summary>
/// Mapper for converting identity document domain entities to data transfer objects.
/// </summary>
public static class IdentityDocumentMapper
{
    /// <summary>
    /// Converts an identity document domain entity to an identity document data transfer object.
    /// </summary>
    /// <param name="document">The identity document domain entity to map.</param>
    /// <returns>An identity document DTO containing the mapped data.</returns>
    public static IdentityDocumentDto ToDto(IdentityDocument document)
    {
        // Map identity document entity properties to DTO
        return new IdentityDocumentDto
        {
            Id = document.Id,
            Name = document.Name,
            Abbreviation = document.Abbreviation,
            Country = document.Country,
            Length = document.Length,
            NumericOnly = document.NumericOnly,
            IsActive = document.IsActive
        };
    }
}
