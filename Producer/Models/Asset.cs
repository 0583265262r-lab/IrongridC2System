using System.ComponentModel.DataAnnotations;

namespace Producer.Models
{
    public class Asset
    {
        public int AssetId { get; set; }
        [RegularExpression("(UAV)|(PerimeterSensor)")]
        public string AssetType { get; set; } = string.Empty;
        public string RawValue { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}
