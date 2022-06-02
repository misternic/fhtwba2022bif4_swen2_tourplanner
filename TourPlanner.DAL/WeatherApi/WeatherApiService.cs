using System.Text.Json;
using Microsoft.Extensions.Configuration;
using TourPlanner.Common;
using TourPlanner.Common.Logging;

namespace TourPlanner.DAL.WeatherApi;

public static class WeatherApiService
{
    private static ILoggerWrapper logger = LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private static readonly HttpClient Client = new();
    private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
    
    public static async Task<float?> GetTemperatureAtDateAsync(string query, DateOnly date)
    {
        try
        {
            var parameters = new Dictionary<string, string>
                {
                    {"key", Config["WeatherApiKey"]},
                    {"q", query},
                    {"dt", date.ToString("yyyy-MM-dd")}
                }
                .Select(param => $"{param.Key}={param.Value}");
            
            var uri = $"https://api.weatherapi.com/v1/history.json?{string.Join("&", parameters)}";
            var response = await Client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            
            var content = await response.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<WeatherApiDto>(content);

            return dto?.Forecast.ForecastDays.ToList()[0].Day.AvgTempC;
        }
        catch (Exception e)
        {
            logger.Error(e.Message);
            return null;
        }
    }
}