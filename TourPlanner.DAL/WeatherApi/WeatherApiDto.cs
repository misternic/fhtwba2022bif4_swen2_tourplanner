using System.Text.Json.Serialization;

namespace TourPlanner.DAL.WeatherApi;

public class WeatherApiDto
{
    [JsonPropertyName("forecast")]
    public Forecast Forecast { get; set; }
}

public class Forecast
{
    [JsonPropertyName("forecastday")]
    public IEnumerable<ForecastDay> ForecastDays { get; set; }
}

public class ForecastDay
{
    [JsonPropertyName("day")]
    public Day Day { get; set; }
}

public class Day
{
    [JsonPropertyName("maxtemp_c")]
    public float MaxTempC { get; set; }
    
    [JsonPropertyName("mintemp_c")]
    public float MinTempC { get; set; }
    
    [JsonPropertyName("avgtemp_c")]
    public float AvgTempC { get; set; }
}