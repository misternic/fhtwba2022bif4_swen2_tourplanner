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
            Console.WriteLine($"Distance between \"{from.ToString()}\" and \"{to.ToString()}\": {distance}km");

            var success = await TourManager.GetRouteImage("sample_tour", from, to);
            Console.WriteLine($"Image of tour saved successfully: {success}");
        }
    }
}