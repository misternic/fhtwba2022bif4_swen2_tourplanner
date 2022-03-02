using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TourPlanner.Lib.BL;
using TourPlanner.Lib.Http;

namespace TourPlanner.Lib
{
    internal class Program
    {
        private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
        
        private static readonly Address From = new Address()
        {
            Road = "Höchstädtplatz",
            Number = "6",
            City = "Vienna"
        };
        
        private static readonly Address To = new Address()
        {
            Road = "Schwedenplatz",
            Number = "",
            City = "Vienna"
        };
        
        public static async Task Main(string[] args)
        {
            var id = Guid.NewGuid();
            var tour = new Tour()
            {
                Id = id,
                Name = "Test Tour",
                Description = "This is a description of the test tour.",
                From = From,
                To = To,
                TransportType = TransportType.Bicycle
            };
            
            var metaData = await MapQuestController.GetRouteMetaData(From, To, tour.TransportType);
            await MapQuestController.GetRouteImage(id.ToString(), From, To);

            tour.Distance = metaData.Distance;
            tour.EstimatedTime = metaData.FormattedTime;
            
            File.WriteAllText($"{Config["PersistenceFolder"]}/{id.ToString()}.json", JsonConvert.SerializeObject(tour));
        }
    }
}