using HemoVida.Core.Response;

namespace HemoVida.Core.Interfaces.Repositories;

public interface IReportRepository
{
    Task<List<BloodStockReport>> GetBloodStockReportAsync();
    Task<List<DonorLast30Days>> GetDonorsLast30DaysReportAsync();
}
