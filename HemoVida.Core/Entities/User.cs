using HemoVida.Core.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemoVida.Core.Entities;

[Table("tb_user")]
public class User : BaseEntity
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
}
