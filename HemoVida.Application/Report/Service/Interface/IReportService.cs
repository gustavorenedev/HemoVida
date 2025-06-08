using HemoVida.Core.Response;

namespace HemoVida.Application.Report.Service.Interface;

public interface IReportService
{
    Task<List<BloodStockReport>> GetReportStock();

    Task<List<DonorLast30Days>> GetReportDonorLast30Days();
}
