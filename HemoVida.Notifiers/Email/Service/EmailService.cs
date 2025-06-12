using HemoVida.Notifiers.DTOs;
using HemoVida.Notifiers.Email.Service.Interface;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace HemoVida.Notifiers.Email.Service;

public class EmailService : IEmailService
{
    private readonly SmtpClient _smtpClient;

    public EmailService(IConfiguration configuration)
    {
        var emailSettings = configuration.GetSection("EmailSettings");

        var host = emailSettings["Host"];
        var port = int.Parse(emailSettings["Port"]!);
        var username = emailSettings["Username"];
        var password = emailSettings["Password"];
        var enableSsl = bool.Parse(emailSettings["EnableSsl"]!);

        _smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(username, password),
            EnableSsl = enableSsl
        };
    }

    public async Task SendEmailAsync(DonationPublisherResponse request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email não pode ser vazio.");

        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Nome não pode ser vazio.");

        var emailBody = GenerateDonationEmailBody(request);

        var mailMessage = new MailMessage("hemovida@hemovida.com", request.Email)
        {
            Subject = "Doação confirmada!! HemoVida",
            Body = emailBody,
            IsBodyHtml = true
        };

        await _smtpClient.SendMailAsync(mailMessage);
    }

    private static string GenerateDonationEmailBody(DonationPublisherResponse donation)
    {
        return $@"
        <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    margin: 0;
                    padding: 0;
                    background-color: #f4f4f4;
                }}
                .email-container {{
                    max-width: 600px;
                    margin: 20px auto;
                    background-color: #ffffff;
                    border-radius: 8px;
                    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
                    overflow: hidden;
                }}
                .email-header {{
                    background-color: #E53935;
                    color: #ffffff;
                    text-align: center;
                    padding: 20px;
                }}
                .email-header h1 {{
                    margin: 0;
                    font-size: 24px;
                }}
                .email-body {{
                    padding: 20px;
                    color: #333333;
                }}
                .email-body p {{
                    margin: 10px 0;
                    line-height: 1.6;
                }}
                .email-body strong {{
                    color: #E53935;
                }}
                .email-footer {{
                    text-align: center;
                    padding: 10px;
                    background-color: #f4f4f4;
                    color: #777777;
                    font-size: 12px;
                }}
            </style>
        </head>
        <body>
            <div class='email-container'>
                <div class='email-header'>
                    <h1>Confirmação de Doação - Hemovida</h1>
                </div>
                <div class='email-body'>
                    <p>Olá {donation.Name},</p>
                    <p>Agradecemos pela sua doação realizada com sucesso no dia <strong>{donation.DonationDate:dd/MM/yyyy}</strong>.</p>
                    <p>🩸 Detalhes da Doação:</p>
                    <ul>
                        <li><strong>Nome:</strong> {donation.Name}</li>
                        <li><strong>Email:</strong> {donation.Email}</li>
                        <li><strong>Tipo Sanguíneo:</strong> {donation.BloodType} {donation.RhFactor}</li>
                        <li><strong>Quantidade Doada:</strong> {donation.MlQuantity} ml</li>
                        <li><strong>Data da Doação:</strong> {donation.DonationDate:dd/MM/yyyy}</li>
                    </ul>
                    <p>Sua doação pode salvar até 3 vidas. 💙</p>
                    <p>Caso tenha dúvidas ou queira acompanhar futuras campanhas, entre em contato conosco.</p>
                </div>
                <div class='email-footer'>
                    <p>© 2025 Hemovida. Todos os direitos reservados.</p>
                </div>
            </div>
        </body>
        </html>";
    }
}