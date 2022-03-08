using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TourPlanner.Common;
using TourPlanner.DAL.MapQuest;

namespace TourPlanner.Console
{
    internal class Program
    {
        private static readonly IConfigurationRoot Config = AppSettings.GetInstance().Configuration;
        
        private static readonly Address From = new()
        {
            Road = "Höchstädtplatz",
            Number = "6",
            City = "Vienna"
        };
        
        private static readonly Address To = new()
        {
            Road = "Schwedenplatz",
            Number = "",
            City = "Vienna"
        };
        
        public static void Main(string[] args)
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
            
            // var metaData = await MapQuestService.GetRouteMetaData(From, To, tour.TransportType);
            // await MapQuestService.GetRouteImage(id.ToString(), From, To);
            //
            // tour.Distance = metaData?.Distance ?? Double.NegativeInfinity;
            // tour.EstimatedTime = metaData?.FormattedTime ?? TimeSpan.Zero;
            //
            // File.WriteAllText($"{Config["PersistenceFolder"]}/{id.ToString()}.json", tour.ToJson());

            var report = new TourReport(tour, new List<TourLog>());
            
            report.ToPdf();
        }
    }
}