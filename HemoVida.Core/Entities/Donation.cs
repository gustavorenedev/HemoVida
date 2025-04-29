using System.ComponentModel.DataAnnotations.Schema;

namespace HemoVida.Core.Entities;

[Table("tb_donation")]
public class Donation : BaseEntity
{
    public int DonorId { get; set; }
    public Donor Donor { get; set; }
    public int MlQuantity { get; set; }
    public DateTime DonationDate { get; set; }
}
