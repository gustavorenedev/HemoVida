using HemoVida.Application.Donation.Request;

namespace HemoVida.Application.Publisher.Service.Interface;

public interface IPublisherService
{
    Task PublishAsync(DonationPublisherRequest request);
}
