using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TourPlanner.Common.DTO;

public class TourDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string From { get; set; }
    
    public string To { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public TransportType TransportType { get; set; }
    
    public double Distance { get; set; }
    
    public TimeSpan EstimatedTime { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public int? Popularity { get; set; }

    public double? ChildFriendlyness { get; set; }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public override string ToString()
    {
        return ToJson();
    }
}
