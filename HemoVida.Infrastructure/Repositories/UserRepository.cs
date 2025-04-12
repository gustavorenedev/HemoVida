using HemoVida.Core.Entities;
using HemoVida.Infrastructure.Persistence;
using HemoVida.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HemoVida.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly HemoVidaDbContext _context;

    public UserRepository(HemoVidaDbContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        return user;
    }
}
