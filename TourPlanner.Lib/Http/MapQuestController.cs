using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TourPlanner.Lib.BL;

namespace TourPlanner.Lib.Http
{
    public static class MapQuestController
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
        private static readonly Dictionary<TransportType, string> RouteTypes = new Dictionary<TransportType, string>()
        {
            {TransportType.Car, "fastest"},
            {TransportType.Bicycle, "bicycle"},
            {TransportType.Walking, "pedestrian"}
        };
        
        public static async Task<MapQuestRouteDto> GetRouteMetaData(Address from, Address to, TransportType type)
        {
            try
            {
                var parameters = new Dictionary<string, string>()
                    {
                        {"key", Config["MapQuestApiKey"]},
                        {"from", from.ToString()},
                        {"to", to.ToString()},
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

        public static async Task<bool> GetRouteImage(string filename, Address from, Address to)
        {
            try
            {
                var parameters = new Dictionary<string, string>()
                    {
                        {"key", Config["MapQuestApiKey"]},
                        {"start", $"{from.ToString()}|flag-start"},
                        {"end", $"{to.ToString()}|flag-end"},
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

                var path = $"{Config["PersistenceFolder"]}/{filename}.jpg";
                File.WriteAllBytes(path, await response.Content.ReadAsByteArrayAsync());

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}