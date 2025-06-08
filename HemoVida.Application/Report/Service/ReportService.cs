using HemoVida.Application.Report.Service.Interface;
using HemoVida.Core.Interfaces.Repositories;
using HemoVida.Core.Response;

namespace HemoVida.Application.Report.Service;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<List<BloodStockReport>> GetReportStock()
    {
        return await _reportRepository.GetBloodStockReportAsync();
    }

    public async Task<List<DonorLast30Days>> GetReportDonorLast30Days()
    {
        return await _reportRepository.GetDonorsLast30DaysReportAsync();
    }
}