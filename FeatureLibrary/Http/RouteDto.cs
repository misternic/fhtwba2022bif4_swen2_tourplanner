using System;

namespace FeatureLibrary.Http
{
    public class RouteResultDto
    {
        public RouteDto Route { get; set; }
    }
    
    public class RouteDto
    {
        public double Distance { get; set; }
        public TimeSpan FormattedTime { get; set; }
    }
}