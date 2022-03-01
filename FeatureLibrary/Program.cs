using System;
using System.Threading.Tasks;
using FeatureLibrary.Http;

namespace FeatureLibrary
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var from = new Address()
            {
                Road = "Höchstädtplatz",
                Number = "6",
                City = "Vienna"
            };

            var to = new Address()
            {
                Road = "Schwedenplatz",
                Number = "",
                City = "Vienna"
            };
            
            var distance = await TourManager.GetRouteDistance(from, to);
            Console.WriteLine(distance);

            await TourManager.GetRouteImage(from, to);
        }
    }
}