using System.Text.RegularExpressions;
using TourPlanner.Common.DTO;
using TourPlanner.Common.PDF;
using TourPlanner.DAL;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.BL
{
    public class TourController
    {
        static readonly TourRepository tourRepository = new TourRepository(DbContext.GetInstance());

        public static IEnumerable<TourDto> GetItems(string filter)
        {
            if (filter == null || filter == string.Empty)
                return tourRepository.Get();
            else
                return tourRepository.Get().ToList().Where(t => Regex.Match(t.ToJson(), filter, RegexOptions.IgnoreCase).Success);
        }

        public static TourDto GetById(Guid id)
        {
            return tourRepository.GetById(id);
        }

        public static bool AddItem(TourDto tourDto)
        {
            return tourRepository.Insert(tourDto);
        }

        public static bool UpdateItem(TourDto tourDto)
        {
            return tourRepository.Update(tourDto);
        }

        public static bool RemoveItem(TourDto tourDto)
        {
            return tourRepository.Delete(tourDto.Id);
        }

        public static bool ExportData(string path)
        {
            // TODO: get data from database, serialize, and write to file
            return true;
        }

        public static bool ImportData(string path)
        {
            // TODO: get json from file, deserialize and send to database
            return true;
        }

        public static bool ExportTourAsPdf(string path, TourDto tour, List<TourLogDto> logs)
        {
            var report = new TourReport(tour, logs);
            return report.ExportToPdf(path);
        }

        public static bool ExportSummaryAsPdf(string path)
        {
            var report = new SummaryReport((ICollection<TourSummaryDto>)tourRepository.GetTourSummaries());
            return report.ExportToPdf(path);
        }
    }
}
