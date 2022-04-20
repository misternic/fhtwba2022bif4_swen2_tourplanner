using TourPlanner.Common;
using TourPlanner.DAL;
using TourPlanner.DAL.MapQuest;
using TourPlanner.DAL.Repositories;

var config = AppSettings.GetInstance().Configuration;

var tour = new Tour
{
    Id = Guid.Parse("e5aa4d9a-4233-4c51-b7eb-727d854f674b"),
    Name = "Test Tour",
    Description = "This is a description of the test tour.",
    From = "Am Graben 19, Vienna, AT",
    To = "Schwedenplatz, Vienna, AT",
    TransportType = TransportType.Bicycle,
    Distance = 0.9399,
    EstimatedTime = new TimeSpan(0, 3, 23)
};

// --------------------------------------------
// Tour Repository
// --------------------------------------------
void TestTourRepository()
{
    var context = DbContext.GetInstance();
    context.Init();
    var tourRepository = new TourRepository(context);

    var insert = tourRepository.Insert(tour);
    var get = tourRepository.Get().ToList();

    tour.Description = "Updated Description.";

    var update = tourRepository.Update(tour);
    var getById = tourRepository.GetById(tour.Id);
    
    Console.WriteLine($"Insert: {insert}");
    Console.WriteLine($"Get: {get.Count}");
    Console.WriteLine($"Update: {update}");
    Console.WriteLine($"Get by ID: {getById.Description}");
}

// --------------------------------------------
// TourLog Repository
// --------------------------------------------
void TestTourLogRepository()
{
    var context = DbContext.GetInstance();
    // context.Init();
    var logRepository = new TourLogRepository(context);

    var log = new TourLog
    {
        Id = Guid.NewGuid(),
        TourId = tour.Id,
        Date = new DateOnly(2022, 1, 1),
        Difficulty = Difficulty.Easy,
        Duration = new TimeSpan(0, 5, 30),
        Rating = 3,
        Comment = "This is a log comment"
    };

    var insert = logRepository.Insert(log);
    var get = logRepository.Get().ToList();

    log.Comment = "This is an updated log comment.";

    var update = logRepository.Update(log);
    var getById = logRepository.GetById(log.Id);
    
    Console.WriteLine($"Insert: {insert}");
    Console.WriteLine($"Get: {get.Count}");
    Console.WriteLine($"Update: {update}");
    Console.WriteLine($"Get by ID: {getById.Comment}");
}

// --------------------------------------------
// MapQuest HTTP Endpoint
// --------------------------------------------
async Task TestMapQuestRequests()
{
    var metaData = await MapQuestService.GetRouteMetaData(tour.From, tour.To, tour.TransportType);
    await MapQuestService.GetRouteImage(tour.Id.ToString(), tour.From, tour.To);

    tour.Distance = metaData?.Distance ?? Double.NegativeInfinity;
    tour.EstimatedTime = metaData?.FormattedTime ?? TimeSpan.Zero;

    File.WriteAllText($"{config["PersistenceFolder"]}/{tour.Id.ToString()}.json", tour.ToJson());
}

// --------------------------------------------
// Report PDF export
// --------------------------------------------
void TestReportExport()
{
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
    var success = report.ExportToPdf();
    Console.WriteLine(success);
}

// --------------------------------------------
// Function calls
// --------------------------------------------

// await TestMapQuestRequests();
// TestTourRepository();
// TestTourLogRepository();
TestReportExport();
