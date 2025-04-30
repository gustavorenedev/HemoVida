using AutoMapper;
using HemoVida.Application.Auth.Request;
using HemoVida.Application.Auth.Response;
using HemoVida.Application.Auth.Service.Interfaces;
using HemoVida.Core.Interfaces.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HemoVida.Application.Auth.Service;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IMapper mapper, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<string> LoginAsync(LoginUserRequest loginUserRequest)  
    {
        if (string.IsNullOrEmpty(loginUserRequest.Email) || string.IsNullOrEmpty(loginUserRequest.Password))
            return null; 

        var user = await _userRepository.GetByEmailAsync(loginUserRequest.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(loginUserRequest.Password, user.Password))
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email!)
        }),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpiresInMinutes"]!)),
            Issuer = _configuration["JwtSettings:Issuer"],
            Audience = _configuration["JwtSettings:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<CreateUserResponse> RegisterAsync(CreateUserRequest userRequest)
    {
        ValidateRequest(userRequest);

        if (await IsEmailInUseAsync(userRequest.Email!))
            throw new InvalidOperationException("Email already in use.");

        var createUser = PrepareUser(userRequest);

        var userCreated = await _userRepository.AddAsync(createUser);

        return _mapper.Map<CreateUserResponse>(userCreated);
    }

    private static void ValidateRequest(CreateUserRequest userRequest)
    {
        if (string.IsNullOrEmpty(userRequest.Email) || string.IsNullOrEmpty(userRequest.Password))
            throw new InvalidOperationException("Email and password are required.");

        if (userRequest.Password != userRequest.ConfirmPassword)
            throw new InvalidOperationException("Confirmation password not supported.");
    }

    private async Task<bool> IsEmailInUseAsync(string email)
    {
        return await _userRepository.GetByEmailAsync(email) != null;
    }

    private Core.Entities.User PrepareUser(CreateUserRequest userRequest)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRequest.Password);

        var userWithHashedPassword = new CreateUserRequest
        {
            Name = userRequest.Name,
            Email = userRequest.Email,
            Password = hashedPassword
        };

        return _mapper.Map<Core.Entities.User>(userWithHashedPassword);
    }
}
