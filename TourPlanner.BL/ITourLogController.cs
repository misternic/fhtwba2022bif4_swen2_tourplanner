using TourPlanner.Common.DTO;

namespace TourPlanner.BL
{
    public interface ITourLogController
    {
        Task<bool> Save(TourLogDto tourLog);
        TourLogDto GetById(Guid id);
        IEnumerable<TourLogDto> GetLogsOfTour(Guid id);
        bool DeleteTourLog(TourLogDto tourLog);
    }
}