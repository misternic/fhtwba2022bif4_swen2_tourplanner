using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TourPlanner.Common;

public class Tour
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string From { get; set; }
    
    public string To { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public TransportType TransportType { get; set; } = TransportType.Bicycle;
    
    public double Distance { get; set; }
    
    public TimeSpan EstimatedTime { get; set; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }
}
