using System.ComponentModel.DataAnnotations;

namespace IronGridC2Api.Models
{
    public class Asset
    {
        public int Id { get; set; }
        public int UnitId { get; set; }
        [Required]
        public string AssetSerial { get; set; } = string.Empty;
        [RegularExpression("(UAV)|(PerimeterSensor)")]
        public string AssetType { get; set; } = "GenericAsset";

        public Unit? Unit { get; set; }
        public AssetLiveStatus LiveStatus { get; set; } = null!;
    }
}

