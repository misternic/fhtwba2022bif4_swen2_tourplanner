using Microsoft.Extensions.Configuration;
using TourPlanner.Common;

namespace TourPlanner.DAL.MapQuest;

public static class MapQuestService
{
    private static readonly HttpClient Client = new();
    private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
    private static readonly Dictionary<TransportType, string> RouteTypes = new()
    {
        {TransportType.Car, "fastest"},
        {TransportType.Bicycle, "bicycle"},
        {TransportType.Walking, "pedestrian"}
    };
    
    public static async Task<MapQuestRouteDto?> GetRouteMetaData(string from, string to, TransportType type)
    {
        try
        {
            var parameters = new Dictionary<string, string>()
                {
                    {"key", Config["MapQuestApiKey"]},
                    {"from", from},
                    {"to", to},
                    {"unit", "k"},
                    {"locale", "de_DE"},
                    {"routeType", RouteTypes[type]}
                }
                .Select(param => $"{param.Key}={param.Value}");
            
            var uri = $"https://www.mapquestapi.com/directions/v2/route?{string.Join("&", parameters)}";
            var response = await Client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            
            var route = await response.Content.ReadAsAsync<MapQuestRouteResultDto>();
            return route.Route;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public static async Task<bool> GetRouteImage(string filename, string from, string to)
    {
        try
        {
            var parameters = new Dictionary<string, string>()
                {
                    {"key", Config["MapQuestApiKey"]},
                    {"start", $"{from}|flag-start"},
                    {"end", $"{to}|flag-end"},
                    {"size", "@2x"},
                    // TODO: find better zoom value (maybe dynamic from distance of route?)   
                }
                .Select(param => $"{param.Key}={param.Value}");
            
            var uri = $"https://www.mapquestapi.com/staticmap/v5/map?{string.Join("&", parameters)}";
            var response = await Client.GetAsync(uri);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            var path = Path.Join(Config["PersistenceFolder"], $"{filename}.jpg");
            await File.WriteAllBytesAsync(path, await response.Content.ReadAsByteArrayAsync());

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
    }
}