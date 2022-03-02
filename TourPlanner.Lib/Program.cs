using System;
using System.Threading.Tasks;
using TourPlanner.Lib.Http;

namespace TourPlanner.Lib
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
            
            var distance = await MapQuestController.GetRouteDistance(from, to);
            Console.WriteLine($"Distance between \"{from.ToString()}\" and \"{to.ToString()}\": {distance}km");

            var success = await MapQuestController.GetRouteImage("sample_tour", from, to);
            Console.WriteLine($"Image of tour saved successfully: {success}");
        }
    }
}