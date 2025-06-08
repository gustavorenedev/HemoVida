using HemoVida.Core.Enum;

namespace HemoVida.Application.Donor.Response;

public class GetDonationHistoryResponse
{
    public string Name { get; set; }
    public Gender? Gender { get; set; }
    public List<DonationsResponse>? Donations { get; set; }
}
