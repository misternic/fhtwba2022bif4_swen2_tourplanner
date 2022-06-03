using TourPlanner.Common.DTO;

namespace TourPlanner.BL
{
    public interface ITourLogController
    {
        Task<bool> AddTourLogAsync(TourLogDto tourLog);
        bool DeleteTourLog(TourLogDto tourLog);
        IEnumerable<TourLogDto> GetLogsOfTour(Guid id);
        bool UpdateTourLog(TourLogDto tourLog);
    }
}