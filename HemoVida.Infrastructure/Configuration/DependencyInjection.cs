using HemoVida.Infrastructure.Repositories;
using HemoVida.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace HemoVida.Infrastructure.Configuration;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();

        // Application Services

        return services;
    }
}
