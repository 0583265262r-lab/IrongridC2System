using Confluent.Kafka;
using System.Text.Json;

public class KafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    public KafkaProducerService(string bootstrapServer)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServer
        };
        _producer = new ProducerBuilder<Null, string>(config).Build();
    }
    public async Task<DeliveryResult<Null, string>> SendAsync<T>(string topicName, T msg)
    {
        var jsonObj = JsonSerializer.Serialize(msg);
        var result = await _producer.ProduceAsync(topicName, new Message<Null, string>
        {
            Value = jsonObj
        });
        return result;

    }
    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}
