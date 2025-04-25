using HemoVida.Core.Entities;

namespace HemoVida.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User> GetByEmailAsync(string email);
}
