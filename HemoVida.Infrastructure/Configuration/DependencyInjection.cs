using HemoVida.Application.Auth.Profile;
using HemoVida.Application.Auth.Service;
using HemoVida.Application.Auth.Service.Interfaces;
using HemoVida.Application.Donor.Service;
using HemoVida.Application.Donor.Service.Interface;
using HemoVida.Application.ZipCode.Service;
using HemoVida.Application.ZipCode.Service.Interface;
using HemoVida.Core.Interfaces;
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
        services.AddScoped<IDonorRepository, DonorRepository>();

        // Application Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDonorService, DonorService>();
        services.AddHttpClient<IZipCodeService, ZipCodeService>();

        // AutoMapper
        services.AddAutoMapper(typeof(AuthenticationProfile));

        return services;
    }
}
