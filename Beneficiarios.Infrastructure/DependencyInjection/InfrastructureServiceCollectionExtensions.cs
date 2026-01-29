using Beneficiarios.Application.Commands.Beneficiarios;
using Beneficiarios.Application.Interfaces.Repositories;
using Beneficiarios.Application.Queries.Beneficiarios;
using Beneficiarios.Application.Queries.Documentos;
using Beneficiarios.Infrastructure.Data;
using Beneficiarios.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Beneficiarios.Infrastructure.DependencyInjection;

/// <summary>
/// Extension methods for registering infrastructure services in the dependency injection container.
/// Registers SQL connection factory, repositories, and application handlers.
/// </summary>
public static class InfrastructureServiceCollectionExtensions
{
    /// <summary>
    /// Adds infrastructure services to the dependency injection container.
    /// Registers database connectivity, repositories, and application command/query handlers.
    /// </summary>
    /// <param name="services">The service collection to add services to.</param>
    /// <param name="configuration">The application configuration for retrieving connection strings.</param>
    /// <returns>The updated service collection for method chaining.</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Register data access layer services
        // SQL connection factory for creating database connections
        services.AddSingleton<ISqlConnectionFactory>(_ =>
            new SqlConnectionFactory(
                configuration.GetConnectionString("BeneficiariosDb")!
            ));

        // Register repository implementations
        // Beneficiary repository for CRUD operations on beneficiaries
        services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
        
        // Identity document repository for retrieving document types
        services.AddScoped<IIdentityDocumentRepository, IdentityDocumentRepository>();

        // Register application handlers
        // Command handlers for creating, updating, and deleting beneficiaries
        services.AddScoped<CreateBeneficiaryCommandHandler>();
        services.AddScoped<UpdateBeneficiaryCommandHandler>();
        services.AddScoped<DeleteBeneficiaryCommandHandler>();
        
        // Query handlers for retrieving beneficiaries and identity documents
        services.AddScoped<GetBeneficiaryByIdQueryHandler>();
        services.AddScoped<GetBeneficiariesQueryHandler>();
        services.AddScoped<GetActiveIdentityDocumentsQueryHandler>();

        return services;
    }
}
