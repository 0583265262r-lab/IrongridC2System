using System.Text.Json;
using Producer.Models;


namespace Producer.Services
{
    public class DataLoaderService
    {
        public List<Asset> LoadJson(string filePath)
        {
            var json = File.ReadAllText(filePath);
            var result = JsonSerializer.Deserialize<List<Asset>>(json) ?? new List<Asset>();
            Console.WriteLine(result[0].AssetType);
            return result;
        }

        public List<Asset> UAVDataFromJson(List<Asset> jsonFile)
        {
            return jsonFile.Where(a => a.AssetType == "UAV").ToList();
        }
        public List<Asset> PerimeterSensorDataFromJson(List<Asset> jsonFile)
        {
            return jsonFile.Where(a => a.AssetType == "PerimeterSensor").ToList();
        }
    }
}
















