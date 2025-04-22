using HemoVida.Core.Enum;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HemoVida.Application.Auth.Request;

public class CreateUserRequest
{
    [Required]
    public string? Name { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }
    [Required]
    public string? ConfirmPassword { get; set; }
    [JsonIgnore]
    public Role Role { get; set; } = Role.User;
}
