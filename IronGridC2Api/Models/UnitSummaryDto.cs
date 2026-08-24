namespace IronGridC2Api.Models
{
    public class UnitSummaryDto
    {
        public int UnitId { get; set; }
        public string UnitName { get; set; }
        public string Sector { get; set; }
        public int TotalAssets { get; set; }
        public int StableAssets { get; set; }
        public int WarningAssets { get; set; }
        public int UnverifiedAssets { get; set; }
    }
}
