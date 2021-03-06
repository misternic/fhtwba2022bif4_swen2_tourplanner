using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace TourPlanner.Common.DTO;

public class TourLogDto
{
    public Guid Id { get; set; }
    
    public Guid TourId { get; set; }
    
    public DateOnly Date { get; set; }

    public string Comment { get; set; } = "";
    
    public int Rating { get; set; }
    
    [JsonConverter(typeof(StringEnumConverter))]
    public Difficulty Difficulty { get; set; }
    
    public TimeSpan Duration { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public float? Temperature { get; set; } = null;

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public override string ToString()
    {
        return ToJson();
    }
}