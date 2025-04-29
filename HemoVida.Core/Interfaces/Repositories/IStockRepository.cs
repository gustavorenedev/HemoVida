namespace HemoVida.Core.Interfaces.Repositories;

public interface IStockRepository
{
    Task<bool> UpdateStockAsync(Core.Entities.Donation donation);
}
