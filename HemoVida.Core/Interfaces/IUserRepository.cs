using HemoVida.Core.Entities;

namespace HemoVida.Infrastructure.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User> GetByEmailAsync(string email);
}
