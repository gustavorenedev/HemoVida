using Confluent.Kafka;
using HemoVida.Notifiers.DTOs;
using HemoVida.Notifiers.Email.Service;
using HemoVida.Notifiers.Email.Service.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace HemoVida.Notifiers;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IConfiguration _configuration;
    private readonly string _topicName;
    private readonly string _kafkaBootstrapServers;
    private readonly string _notifierConsumeGroupName;

    public Worker(ILogger<Worker> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;

        _topicName = _configuration["HemovidaSettings:HemovidaTopicName"];
        _kafkaBootstrapServers = _configuration["HemovidaSettings:KafkaBootstrapServer"];
        _notifierConsumeGroupName = _configuration["HemovidaSettings:NotifierConsumeGroupName"];
    }

    /// <summary>
    /// Lopping process
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started and waiting for messages...");

        IEmailService emailService = new EmailService(_configuration);

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Hemovida notifier running at: {time}", DateTimeOffset.Now);

            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaBootstrapServers,
                GroupId = _notifierConsumeGroupName,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(_topicName);

                _logger.LogInformation("Subscribe in topic {topic}", _topicName);

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) => {
                    e.Cancel = true;
                    cts.Cancel();
                };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var consumeResult = consumer.Consume(cts.Token);

                            _logger.LogInformation("Message received: {message}", consumeResult.Message.Value);

                            DonationPublisherResponse donation = JsonSerializer.Deserialize<DonationPublisherResponse>(consumeResult.Message.Value)!;

                            await emailService.SendEmailAsync(donation);

                            Console.WriteLine($"Processed message: {consumeResult.Message.Value}");
                        }
                        catch (ConsumeException e)
                        {
                            _logger.LogError("Error consuming message: {error}", e.Error.Reason);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Worker cancelled. Closing consumer...");
                    consumer.Close();
                }
            }
        }
    }
}