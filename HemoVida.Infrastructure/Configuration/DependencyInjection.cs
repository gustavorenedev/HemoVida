using HemoVida.Application.Auth.Profile;
using HemoVida.Application.Auth.Service;
using HemoVida.Application.Auth.Service.Interfaces;
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
        services.AddScoped<IAuthService, AuthService>();

        // AutoMapper
        services.AddAutoMapper(typeof(AuthenticationProfile));

        return services;
    }
}
