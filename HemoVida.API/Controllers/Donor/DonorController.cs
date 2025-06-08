using HemoVida.Application.Donor.Request;
using HemoVida.Application.Donor.Response;
using HemoVida.Application.Donor.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HemoVida.API.Controllers.Donor;

/// <summary>
/// Controlador para registro das informações de doadores e doação.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DonorController : ControllerBase
{
    private readonly IDonorService _donorService;

    /// <summary>
    /// Construtor do DonorController.
    /// </summary>
    /// <param name="donorService">Serviço de registro das informações de doadores e doação.</param>
    public DonorController(IDonorService donorService)
    {
        _donorService = donorService;
    }

    /// <summary>
    /// Endpoint para mostrar os doadores disponíveis na clinica que ainda não doaram.
    /// </summary>
    /// <returns>Retorna os doadores disponíveis na clinica que ainda não doaram</returns>
    /// <response code="200">Doadores retornados com sucesso.</response>
    [HttpGet("GetAvailableDonors")]
    [ProducesResponseType(typeof(List<GetAvailableDonorsResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]

    public async Task<IActionResult> GetAvailableDonors()
    {
        var response = await _donorService.GetAvailableDonors();

        return Ok(response);
    }

    /// <summary>
    /// Endpoint para preencher informações de prancheta pré doação.
    /// </summary>
    /// <param name="donorRequest">Dados do doador a serem registrados.</param>
    /// <response code="201">Doador registrado com sucesso.</response>
    /// <response code="400">Erro de validação nos dados enviados.</response>
    [HttpPost("RegisterDonor")]
    [ProducesResponseType(typeof(CreateDonorResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]

    public async Task<IActionResult> Register([FromBody] CreateDonorRequest donorRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest("Dados inválidos.");

        var response = await _donorService.RegisterDonor(donorRequest);

        return Ok(response.Message);
    }

    /// <summary>
    /// Endpoint para mostrar as doações feitas pelo doador.
    /// </summary>
    /// <returns>Retorna as doações feitas pelo doador</returns>
    /// <response code="200">Doadores retornados com sucesso.</response>
    [HttpGet("GetDonationHistory")]
    [ProducesResponseType(typeof(GetDonationHistoryResponse), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]

    public async Task<IActionResult> GetDonationHistory(string email)
    {
        var response = await _donorService.GetDonationHistory(email);

        return Ok(response);
    }

    /// <summary>
    /// Endpoint para mostrar todas as doações feitas pelos doadores.
    /// </summary>
    /// <returns>Retorna as doações feitas pelos doadores</returns>
    /// <response code="200">Doadores retornados com sucesso.</response>
    [HttpGet("GetAllDonationHistory")]
    [ProducesResponseType(typeof(List<GetDonationHistoryResponse>), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]

    public async Task<IActionResult> GetAllDonationHistory()
    {
        var response = await _donorService.GetDonationHistory();

        return Ok(response);
    }
}
