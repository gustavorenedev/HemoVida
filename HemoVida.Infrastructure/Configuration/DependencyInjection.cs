using HemoVida.Application.Auth.Profile;
using HemoVida.Application.Auth.Service;
using HemoVida.Application.Auth.Service.Interfaces;
using HemoVida.Application.Donor.Service;
using HemoVida.Application.Donor.Service.Interface;
using HemoVida.Application.ZipCode.Service;
using HemoVida.Application.ZipCode.Service.Interface;
using HemoVida.Core.Interfaces.Repositories;
using HemoVida.Core.Interfaces.Service;
using HemoVida.Infrastructure.Redis;
using HemoVida.Infrastructure.Repositories;
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
        services.AddScoped<IRedisService, RedisService>();
        services.AddHttpClient<IZipCodeService, ZipCodeService>();

        // AutoMapper
        services.AddAutoMapper(typeof(AuthenticationProfile));

        return services;
    }
}
