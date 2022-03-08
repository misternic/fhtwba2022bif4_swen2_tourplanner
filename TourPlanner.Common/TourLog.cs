using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TourPlanner.Common
{
    public class TourLog
    {
        public Guid Id { get; set; }
        
        public DateTime Date { get; set; }
        
        public string Comment { get; set; }
        
        public int Rating { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public Difficulty Difficulty { get; set; }
        
        public TimeSpan Duration { get; set; }
    }
}