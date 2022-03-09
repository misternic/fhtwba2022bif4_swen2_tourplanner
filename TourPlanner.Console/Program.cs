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

var logs = new List<TourLog>
{
    new()
    {
        Date = new DateOnly(2022, 3, 1),
        Comment = "This is a comment.",
        Rating = 4,
        Difficulty = Difficulty.Easy,
        Duration = new TimeSpan(0, 5, 23)
    },
    new()
    {
        Date = new DateOnly(2022, 3, 2),
        Rating = 3,
        Difficulty = Difficulty.Hard,
        Duration = new TimeSpan(0, 10, 59)
    },
    new()
    {
        Date = new DateOnly(2022, 3, 3),
        Comment = "This was extremely hard and I don't want to do it again.",
        Rating = 1,
        Difficulty = Difficulty.Extreme,
        Duration = new TimeSpan(4, 2, 0)
    },
    new()
    {
        Date = new DateOnly(2022, 3, 4),
        Rating = 5,
        Comment = "Easy Peasy",
        Difficulty = Difficulty.Easy,
        Duration = new TimeSpan(0, 3, 59)
    }
};

var report = new TourReport(tour, logs);
report.ToPdf();