using Microsoft.Extensions.Configuration;
using Producer.Services;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();
var bootstrapServer = configuration["Kafka:BootsrapServers"] ?? "localhost:9092";
var uavTopic = configuration["Kafka:Topic:UAV"] ?? "uav";
var perimeterSensorTopic = configuration["Kafka:Topic:PerimeterSensor"] ?? "PerimeterSensors";


var loadJson = new DataLoaderService();

var jsonFile = loadJson.LoadJson("Data/field_reports.json");
var uav = loadJson.UAVDataFromJson(jsonFile);

var perimeterSensor = loadJson.PerimeterSensorDataFromJson(jsonFile);


var ghgg = loadJson.UAVDataFromJson(uav);

var producer = new KafkaProducerService(bootstrapServer);

foreach (var u in uav)
{
    await producer.SendAsync(uavTopic, u);
}
foreach (var p in perimeterSensor)
{
    await producer.SendAsync(perimeterSensorTopic, p);
}
