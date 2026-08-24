using Confluent.Kafka;
using Consumer.Data;
using Consumer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
var service = new ServiceCollection();
service.AddDbContext<IronGridDbContext>(Options =>
        Options.UseMySql(
            configuration.GetConnectionString("testDb"),
            ServerVersion.AutoDetect(configuration.GetConnectionString("testDb"))));
service.AddScoped<UavProcessingService>();
var serviceProvider = service.BuildServiceProvider();

using (var scope = serviceProvider.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IronGridDbContext>();
    db.Database.EnsureCreated();
}
var consumerConfig = new ConsumerConfig
{
    BootstrapServers = configuration["Kafka:BootsrapServers"],
    GroupId = "uav-consumer-group",
    AutoOffsetReset = AutoOffsetReset.Earliest,
    EnableAutoCommit = false
};

using var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();


var uavTopic = configuration["Kafka:Topic:UAV"] ?? "uav";
var perimeterSensorTopic = configuration["Kafka:Topic:PerimeterSensor"] ?? "PerimeterSensors";


try
{
    consumer.Subscribe(uavTopic);
    while (true)
    {
        var result = consumer.Consume(TimeSpan.FromSeconds(10));

        if (result == null || result.Message?.Value == null)
        {
            break;
        }

        using var scope = serviceProvider.CreateScope();
        var processingService = scope.ServiceProvider.GetRequiredService<UavProcessingService>();
        if (await processingService.ProcessUavAsync(result.Message.Value))
        {
            Console.WriteLine(result.Message.Value);
            consumer.Commit(result);
        }
    }
    consumer.Unsubscribe();
    await Task.Delay(TimeSpan.FromSeconds(10));



    consumer.Subscribe(perimeterSensorTopic);
    while (true)
    {
        var result = consumer.Consume(TimeSpan.FromSeconds(10));
        if (result == null || result.Message?.Value == null)
        {
            break;
        }
        using var scope = serviceProvider.CreateScope();
        var processingService = scope.ServiceProvider.GetRequiredService<UavProcessingService>();
        if (await processingService.ProcessPerimeterSensorAsync(result.Message.Value))
        {
            Console.WriteLine(result.Message.Value);
            consumer.Commit(result);
        }
    }

}
catch (Exception ex)
{
    Console.WriteLine(ex);
}
finally
{
    consumer.Close();
    Console.WriteLine("Consumer closed.");
}




























//consumer.Unsubscribe();
//Console.WriteLine("Waiting for delay period...");
//await Task.Delay(TimeSpan.FromSeconds(10));



//consumer.Subscribe(tracksTopic);
//Console.WriteLine($"Consuming Topic {tracksTopic}...");
//while (true)
//{
//    var result = consumer.Consume(TimeSpan.FromSeconds(10));
//    if (result == null || result.Message?.Value == null)
//        continue;

//    using var scope = serviceProvider.CreateScope();
//    var processingService = scope.ServiceProvider.GetRequiredService<HostileUavProcessingService>();
//    if (await processingService.ProcessTracksAsync(result.Message.Value))
//    {
//        Console.WriteLine(result.Message.Value);
//        consumer.Commit(result);
//    }
//}