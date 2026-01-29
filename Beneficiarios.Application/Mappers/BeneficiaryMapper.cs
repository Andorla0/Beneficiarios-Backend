using Beneficiarios.Application.Dtos;
using Beneficiarios.Domain.Entities;

namespace Beneficiarios.Application.Mappers;

/// <summary>
/// Mapper for converting beneficiary domain entities to data transfer objects.
/// </summary>
public static class BeneficiaryMapper
{
    /// <summary>
    /// Converts a beneficiary domain entity to a beneficiary data transfer object.
    /// </summary>
    /// <param name="beneficiary">The beneficiary domain entity to map.</param>
    /// <returns>A beneficiary DTO containing the mapped data.</returns>
    public static BeneficiaryDto ToDto(Beneficiary beneficiary)
    {
        // Map beneficiary entity properties to DTO
        return new BeneficiaryDto
        {
            Id = beneficiary.Id,
            FirstNames = beneficiary.FirstNames,
            LastNames = beneficiary.LastNames,
            IdentityDocumentId = beneficiary.IdentityDocumentId,
            DocumentNumber = beneficiary.DocumentNumber,
            BirthDate = beneficiary.BirthDate,
            Gender = beneficiary.Gender
        };
    }
}
