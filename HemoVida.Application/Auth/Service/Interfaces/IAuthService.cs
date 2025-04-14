using HemoVida.Application.Auth.Request;
using HemoVida.Application.Auth.Response;

namespace HemoVida.Application.Auth.Service.Interfaces;

public interface IAuthService
{
    Task<string> LoginAsync(LoginUserRequest loginUserRequest);
    Task<CreateUserResponse> RegisterAsync(CreateUserRequest userRequest);
}