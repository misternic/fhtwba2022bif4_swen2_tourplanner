using TourPlanner.Common.DTO;

namespace TourPlanner.DAL.Repositories
{
    public interface ITourRepository : IRepository<TourDto>
    {
        IEnumerable<TourSummaryDto> GetTourSummaries();
    }
}