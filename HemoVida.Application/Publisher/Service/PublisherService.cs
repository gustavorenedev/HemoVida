using Confluent.Kafka;
using HemoVida.Application.Donation.Request;
using HemoVida.Application.Publisher.Service.Interface;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace HemoVida.Application.Publisher.Service;

public class PublisherService : IPublisherService
{
    private readonly IConfiguration _configuration;
    private readonly string _kafkaBootstrapServers;
    private readonly string _topicName;

    public PublisherService(IConfiguration configuration)
    {
        _configuration = configuration;
        _kafkaBootstrapServers = _configuration["KafkaSettings:KafkaBootstrapServer"];
        _topicName = _configuration["KafkaSettings:TopicName"];
    }

    public async Task PublishAsync(DonationPublisherRequest request)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaBootstrapServers
        };

        string json = JsonSerializer.Serialize(request);

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            await producer.ProduceAsync(_topicName, new Message<Null, string> { Value = json });
        }
    }
}
