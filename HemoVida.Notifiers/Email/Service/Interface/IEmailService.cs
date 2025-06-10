using HemoVida.Notifiers.DTOs;

namespace HemoVida.Notifiers.Email.Service.Interface;

public interface IEmailService
{
    Task SendEmailAsync(DonationPublisherResponse request);
}
