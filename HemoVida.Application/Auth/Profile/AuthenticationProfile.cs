using HemoVida.Application.Auth.Request;
using HemoVida.Application.Auth.Response;

namespace HemoVida.Application.Auth.Profile;

public class AuthenticationProfile : AutoMapper.Profile
{
    public AuthenticationProfile()
    {
        CreateMap<CreateUserRequest, Core.Entities.User>();
        CreateMap<CreateUserResponse, Core.Entities.User>();
        CreateMap<Core.Entities.User, CreateUserRequest>();
        CreateMap<Core.Entities.User, CreateUserResponse>();
    }
}
