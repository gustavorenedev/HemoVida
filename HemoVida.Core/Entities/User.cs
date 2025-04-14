using HemoVida.Core.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoVida.Core.Entities;

[Table("tb_User")]
public class User : BaseEntity
{
    public User(string email, string password, Role role)
    {
        Email = email;
        Password = password;
        Role = role;
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}
