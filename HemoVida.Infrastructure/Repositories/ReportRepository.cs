using HemoVida.Core.Interfaces.Repositories;
using HemoVida.Core.Response;
using HemoVida.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace HemoVida.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly HemoVidaDbContext _context;

    public ReportRepository(HemoVidaDbContext context)
    {
        _context = context;
    }

    public async Task<List<BloodStockReport>> GetBloodStockReportAsync()
    {
        return await _context.Stocks
            .GroupBy(s => new { s.BloodType, s.RhFactor })
            .Select(g => new BloodStockReport
            {
                BloodType = g.Key.BloodType,
                RhFactor = g.Key.RhFactor,
                TotalMlQuantity = g.Sum(s => s.MlQuantity)
            })
            .ToListAsync();
    }

    public async Task<List<DonorLast30Days>> GetDonorsLast30DaysReportAsync()
    {
        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);

        return await _context.Donations
            .Where(d => d.DonationDate >= thirtyDaysAgo)
            .Include(d => d.Donor)
                .ThenInclude(d => d.User)
            .Select(d => new DonorLast30Days
            {
                DonorName = d.Donor.User.Name,
                BloodType = d.Donor.BloodType,
                RhFactor = d.Donor.RhFactor,
                DonationDate = d.DonationDate,
                MlQuantity = d.MlQuantity
            })
            .ToListAsync();
    }
}