namespace TourPlanner.Common.DTO;

public class TourSummaryDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public float AvgRating { get; set; }
    
    public float AvgDifficulty { get; set; }
    
    public TimeSpan AvgDuration { get; set; }
}