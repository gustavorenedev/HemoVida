using System.ComponentModel.DataAnnotations.Schema;

namespace HemoVida.Core.Entities;

[Table("tb_Donation")]
public class Donation : BaseEntity
{
    public Donation(int donorId, Donor donor, DateTime donationDate)
    {
        DonorId = donorId;
        Donor = donor;
        DonationDate = donationDate;
    }

    public int DonorId { get; set; }
    public Donor Donor { get; set; }
    public DateTime DonationDate { get; set; }
}
