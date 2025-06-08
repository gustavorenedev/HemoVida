using HemoVida.Application.Report.Service.Interface;
using HemoVida.Core.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HemoVida.API.Controllers.Report;

/// <summary>
/// Controlador para relatórios.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    /// <summary>
    /// Construtor do ReportController.
    /// </summary>
    /// <param name="reportService">Serviço para relatórios.</param>
    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    /// <summary>
    /// Endpoint para obter o relatório de estoque de sangue por tipo sanguíneo.
    /// </summary>
    /// <returns>Retorna a quantidade total de sangue disponível por tipo sanguíneo e fator Rh.</returns>
    /// <response code="200">Relatório de estoque retornado com sucesso.</response>
    /// <response code="400">Erro ao gerar o relatório de estoque.</response>
    [HttpGet("GetBloodStockReport")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<BloodStockReport>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetBloodStockReport()
    {
        var response = await _reportService.GetReportStock();
        return Ok(response);
    }

    /// <summary>
    /// Endpoint para obter o relatório de doações feitas nos últimos 30 dias.
    /// </summary>
    /// <returns>Retorna informações dos doadores e doações realizadas nos últimos 30 dias.</returns>
    /// <response code="200">Relatório de doadores retornado com sucesso.</response>
    /// <response code="400">Erro ao gerar o relatório de doadores.</response>
    [HttpGet("GetDonorsLast30DaysReport")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(List<DonorLast30Days>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetDonorsLast30DaysReport()
    {
        var response = await _reportService.GetReportDonorLast30Days();
        return Ok(response);
    }
}