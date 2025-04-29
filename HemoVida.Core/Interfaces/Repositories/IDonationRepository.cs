using HemoVida.Core.Entities;

namespace HemoVida.Core.Interfaces.Repositories;

public interface IDonationRepository
{
    Task<bool> RegisterDonationAsync(Donation donation);
}
