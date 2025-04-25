using System.ComponentModel.DataAnnotations;

namespace HemoVida.Application.Donor.Response;

public class GetAvailableDonorsResponse
{
    public string Name { get; set; }
    public string Weight { get; set; }
    public string BloodType { get; set; }
    public string RhFactor { get; set; }
    [DataType(DataType.Date)]
    public DateTime BirthDate { get; set; }
}
