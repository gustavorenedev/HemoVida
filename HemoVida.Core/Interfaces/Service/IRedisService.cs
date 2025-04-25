using HemoVida.Core.Entities;

namespace HemoVida.Core.Interfaces.Service;

public interface IRedisService
{
    Task AddAvailableDonorAsync(Donor donor);
    Task<List<Donor>> GetAvailableDonorsAsync();
    Task RemoveDonorAsync(int donorId);
}
