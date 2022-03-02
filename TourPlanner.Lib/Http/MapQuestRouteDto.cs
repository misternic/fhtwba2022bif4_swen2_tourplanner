using System;

namespace TourPlanner.Lib.Http
{
    public class MapQuestRouteResultDto
    {
        public MapQuestRouteDto Route { get; set; }
    }
    
    public class MapQuestRouteDto
    {
        public double Distance { get; set; }
        public TimeSpan FormattedTime { get; set; }
    }
}