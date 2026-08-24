using System.ComponentModel.DataAnnotations;

namespace IronGridC2Api.Models
{
    public class AssetStatusDto
    {
        public int AssetId { get; set; }
        public string AssetSerial { get; set; }

        public string AssetType { get; set; } = string.Empty;
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string Sector { get; set; }

        public string ?RawValue { get; set; } = string.Empty;
    
        public string ?ProcessedStatus { get; set; } = string.Empty;

        public bool ?IsVerified { get; set; }

        public DateTime? LastUpdate { get; set; }
    }
}
