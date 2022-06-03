using TourPlanner.Common.DTO;

namespace TourPlanner.BL
{
    public interface ITourController
    {
        bool AddItem(TourDto tourDto);
        bool ExportData(string path);
        bool ExportSummaryAsPdf(string path);
        bool ExportTourAsPdf(string path, TourDto tour, List<TourLogDto> logs);
        TourDto GetById(Guid id);
        IEnumerable<TourDto> GetItems(string filter);
        Task<bool> GetRouteImage(TourDto tour);
        bool ImportData(string path);
        bool RemoveItem(TourDto tourDto);
        Task<bool> UpdateItem(TourDto tour);
    }
}