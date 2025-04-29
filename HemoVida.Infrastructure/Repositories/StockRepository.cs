using HemoVida.Core.Entities;
using HemoVida.Core.Interfaces.Repositories;
using HemoVida.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HemoVida.Infrastructure.Repositories;

public class StockRepository : IStockRepository
{
    private readonly HemoVidaDbContext _context;

    public StockRepository(HemoVidaDbContext context)
    {
        _context = context;
    }

    public async Task<bool> UpdateStockAsync(Donation donation)
    {
        var existingStock = await _context.Stocks
            .FirstOrDefaultAsync(s => s.BloodType == donation.Donor.BloodType);

        if (existingStock != null)
        {
            existingStock.MlQuantity += donation.MlQuantity;
            _context.Stocks.Update(existingStock);
        }
        else
        {
            var newStock = new Stock
            {
                BloodType = donation.Donor.BloodType,
                MlQuantity = donation.MlQuantity,
                RhFactor = donation.Donor.RhFactor
            };
            await _context.Stocks.AddAsync(newStock);
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
