using HemoVida.Application.Donation.Request;
using HemoVida.Application.Donation.Response;
using HemoVida.Application.Donation.Service.Interface;
using HemoVida.Application.Donor.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HemoVida.API.Controllers.Donation;

/// <summary>
/// Controlador para manipular o registro de doações feitas pelos doadores.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class DonationController : ControllerBase
{
    private readonly IDonationService _donationService;

    /// <summary>
    /// Construtor do DonationController.
    /// </summary>
    /// <param name="donationService">Serviço para manipular o registro de doações feitas pelos doadores.</param>
    public DonationController(IDonationService donationService)
    {
        _donationService = donationService;
    }

    /// <summary>
    /// Endpoint para efetivar a doação do doador.
    /// </summary>
    /// <param name="request">Dados da doação a ser registrada.</param>
    /// <response code="201">Doação registrada com sucesso.</response>
    /// <response code="400">Erro de validação nos dados enviados.</response>
    [HttpPost("DonationRegister")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(DonationRegisterResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DonationRegister(DonationRegisterRequest request)
    {
        var response = await _donationService.DonationRegister(request);

        return Ok(response);
    }

    /// <summary>
    /// Endpoint para enviar pedido de doação do doador.
    /// </summary>
    /// <param name="email">email para efetivar o pedido da doação.</param>
    /// <response code="201">Doação pedida com sucesso.</response>
    /// <response code="400">Erro de validação nos dados enviados.</response>
    [HttpPost("DonationRequested")]
    [Authorize(Roles = "User")]
    [ProducesResponseType(typeof(DonationRequestedResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> DonationRequested(string email)
    {
        var response = await _donationService.DonationRequested(email);

        return Ok(response);
    }
}
