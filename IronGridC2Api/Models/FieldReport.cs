using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace IronGridC2Api.Models
{
    public class FieldReport
    {
        public int AssetId { get; set; }

        public string AssetType { get; set; } = string.Empty;

        public string RawValue { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; }
    }
}
