using HemoVida.Core.Entities;
using HemoVida.Core.Interfaces.Repositories;
using HemoVida.Infrastructure.Persistence;

namespace HemoVida.Infrastructure.Repositories;

public class DonationRepository : IDonationRepository
{
    private readonly HemoVidaDbContext _context;

    public DonationRepository(HemoVidaDbContext context)
    {
        _context = context;
    }

    public async Task<bool> RegisterDonationAsync(Donation donation)
    {
        await _context.Donations.AddAsync(donation);
        await _context.SaveChangesAsync();
        return true;
    }
}
