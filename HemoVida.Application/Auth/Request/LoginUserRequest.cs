using System.ComponentModel.DataAnnotations;

namespace HemoVida.Application.Auth.Request;

public class LoginUserRequest
{
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required]
    public string? Password { get; set; }
}
