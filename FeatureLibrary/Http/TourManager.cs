using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FeatureLibrary.Http
{
    public static class TourManager
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly AppSettings AppSettings = AppSettings.GetInstance();
        
        public static async Task<double> GetRouteDistance(Address from, Address to)
        {
            try
            {
                var parameters = new Dictionary<string, string>()
                {
                    {"key", AppSettings.Root["MapQuestApiKey"]},
                    {"from", from.ToString()},
                    {"to", to.ToString()},
                    {"unit", "k"},
                    {"locale", "de_DE"}
                };

                var query = string.Join("&", parameters.Select(param => $"{param.Key}={param.Value}"));

                var uri = $"https://www.mapquestapi.com/directions/v2/route?{query}";
                var response = await Client.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    return -1;
                }
                
                var route = await response.Content.ReadAsAsync<RouteResultDto>();
                return route.Route.Distance;
            }
            catch (HttpRequestException e)
            {
                return -1;
            }
        }

        public static async Task<bool> GetRouteImage(string filename, Address from, Address to)
        {
            try
            {
                var parameters = new Dictionary<string, string>()
                {
                    {"key", AppSettings.Root["MapQuestApiKey"]},
                    {"start", $"{from.ToString()}|flag-start"},
                    {"end", $"{to.ToString()}|flag-end"}
                };

                var query = string.Join("&", parameters.Select(param => $"{param.Key}={param.Value}"));

                var uri = $"https://www.mapquestapi.com/staticmap/v5/map?{query}";
                var response = await Client.GetAsync(uri);

                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                File.WriteAllBytes(
                    $"{AppSettings.Root["PersistenceFolder"]}/{filename}.jpg",
                    await response.Content.ReadAsByteArrayAsync());

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