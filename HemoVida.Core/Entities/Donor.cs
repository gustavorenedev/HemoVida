using System.ComponentModel.DataAnnotations.Schema;

namespace HemoVida.Core.Entities;

[Table("tb_donor")]
public class Donor : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime BirthDate { get; set; }
    public double Weight { get; set; }
    public string BloodType { get; set; }
    public string RhFactor { get; set; }
    public Address Address { get; set; }
    public List<Donation>? Donations { get; set; }
}
