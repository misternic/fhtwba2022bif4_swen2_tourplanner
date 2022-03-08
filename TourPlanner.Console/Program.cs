using TourPlanner.Common;
using TourPlanner.DAL.MapQuest;

var config = AppSettings.GetInstance().Configuration;
        
var from = new Address()
{
    Road = "Am Graben",
    Number = "19",
    City = "Vienna"
};
        
var to = new Address()
{
    Road = "Schwedenplatz",
    Number = "",
    City = "Vienna"
};
        

var id = Guid.Parse("c2623f65-46ab-4ffa-917a-aedc3d3f9dd2");
var tour = new Tour()
{
    Id = id,
    Name = "Test Tour",
    Description = "This is a description of the test tour.",
    From = from,
    To = to,
    TransportType = TransportType.Bicycle,
    Distance = 0.9399,
    EstimatedTime = new TimeSpan(0, 3, 23)
};
        
// var metaData = await MapQuestService.GetRouteMetaData(from, to, tour.TransportType);
// await MapQuestService.GetRouteImage(id.ToString(), from, to);

// tour.Distance = metaData?.Distance ?? Double.NegativeInfinity;
// tour.EstimatedTime = metaData?.FormattedTime ?? TimeSpan.Zero;

// File.WriteAllText($"{config["PersistenceFolder"]}/{id.ToString()}.json", tour.ToJson());

var report = new TourReport(tour, new List<TourLog>());
report.ToPdf();