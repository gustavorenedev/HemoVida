namespace HemoVida.Core.Response;

public class DonorLast30Days
{
    public string DonorName { get; set; }
    public string BloodType { get; set; }
    public string RhFactor { get; set; }
    public DateTime DonationDate { get; set; }
    public int MlQuantity { get; set; }
}
