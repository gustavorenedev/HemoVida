using HemoVida.Application.Auth.Request;
using HemoVida.Application.Auth.Response;
using HemoVida.Application.Auth.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HemoVida.API.Controllers.Auth;

/// <summary>
/// Controlador para autenticação e registro de usuários.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    /// <summary>
    /// Construtor do AuthController.
    /// </summary>
    /// <param name="authService">Serviço de autenticação e registro.</param>
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Endpoint para registrar um novo usuário.
    /// </summary>
    /// <param name="userRequest">Dados do usuário a serem registrados.</param>
    /// <returns>O usuário criado com o ID gerado.</returns>
    /// <response code="201">Usuário criado com sucesso.</response>
    /// <response code="400">Erro de validação nos dados enviados.</response>
    [HttpPost("Register")]
    [ProducesResponseType(typeof(CreateUserResponse), (int)HttpStatusCode.Created)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest userRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest("Dados inválidos.");

        var response = await _authService.RegisterAsync(userRequest);

        return CreatedAtAction(nameof(Register), new { id = response.Id }, response);
    }

    /// <summary>
    /// Endpoint para autenticação de um usuário.
    /// </summary>
    /// <param name="loginUserRequest">Credenciais do usuário para login.</param>
    /// <returns>Token de autenticação.</returns>
    /// <response code="200">Login realizado com sucesso.</response>
    /// <response code="401">Credenciais inválidas.</response>
    [HttpPost("Login")]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(string), (int)HttpStatusCode.Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginUserRequest loginUserRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest("Dados inválidos.");

        var token = await _authService.LoginAsync(loginUserRequest);

        if (string.IsNullOrEmpty(token))
            return Unauthorized("Credenciais inválidas.");

        return Ok(token);
    }
}
