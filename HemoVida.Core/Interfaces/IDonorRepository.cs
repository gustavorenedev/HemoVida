using HemoVida.Core.Entities;

namespace HemoVida.Core.Interfaces;

public interface IDonorRepository
{
    Task<Donor> CreateDonor(Donor donor);
    Task UpdateDonor(Donor donor, int id);
    Task<Donor> GetByEmailAsync(string email);
}
