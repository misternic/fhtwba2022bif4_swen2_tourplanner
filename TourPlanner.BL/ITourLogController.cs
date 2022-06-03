using TourPlanner.Common.DTO;

namespace TourPlanner.BL
{
    public interface ITourLogController
    {
        Task<bool> Save(TourLogDto tourLog);
        Task<bool> AddTourLog(TourLogDto tourLog);
        TourLogDto GetById(Guid id);
        IEnumerable<TourLogDto> GetLogsOfTour(Guid id);
        Task<bool> UpdateTourLog(TourLogDto tourLog);
        bool DeleteTourLog(TourLogDto tourLog);
    }
}