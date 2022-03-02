using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TourPlanner.Lib.BL
{
    public class Tour
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public Address From { get; set; }
        
        public Address To { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TransportType TransportType { get; set; } = TransportType.Bicycle;
        
        public double Distance { get; set; }
        
        public TimeSpan EstimatedTime { get; set; }
    }
}