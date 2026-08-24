using System.ComponentModel.DataAnnotations;

namespace IronGridC2Api.Models
{
    public class Unit
    {
        public int Id { get; set; }
        public string UnitName { get; set; } = "Unknown Unit";
        public string Sector { get; set; } = "General";

        public ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}
