using HemoVida.Notifiers.Email.Service;
using HemoVida.Notifiers.Email.Service.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HemoVida.Notifiers;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddScoped<IEmailService, EmailService>();
                services.AddHostedService<Worker>();
            });
}
