using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace TourPlanner.Lib.Http
{
    public static class MapQuestController
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
        
        public static async Task<double> GetRouteDistance(Address from, Address to)
        {
            try
            {
                var parameters = new Dictionary<string, string>()
                    {
                        {"key", Config["MapQuestApiKey"]},
                        {"from", from.ToString()},
                        {"to", to.ToString()},
                        {"unit", "k"},
                        {"locale", "de_DE"}
                    }
                    .Select(param => $"{param.Key}={param.Value}");
                
                var uri = $"https://www.mapquestapi.com/directions/v2/route?{string.Join("&", parameters)}";
                var response = await Client.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    return -1.0;
                }
                
                var route = await response.Content.ReadAsAsync<MapQuestRouteResultDto>();
                return route.Route.Distance;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return -1.0;
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
                        {"end", $"{to.ToString()}|flag-end"}
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