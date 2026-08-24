using System.ComponentModel.DataAnnotations;

namespace Consumer.Models
{
    public class Units
    {
        public int Id { get; set; }
        public string UnitName { get; set; } = "Unknown Unit";
        public string Sector { get; set; } = "General";

        public ICollection<Asset> Assets { get; set; } = new List<Asset>();
    }
}
