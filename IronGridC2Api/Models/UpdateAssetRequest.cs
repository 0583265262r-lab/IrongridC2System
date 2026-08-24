using System.ComponentModel.DataAnnotations;

namespace IronGridC2Api.Models
{
    public class UpdateAssetRequest
    {

        public int UnitId { get; set; }

        public string AssetSerial { get; set; } = string.Empty;

        public string AssetType { get; set; } = string.Empty;
    }
}
