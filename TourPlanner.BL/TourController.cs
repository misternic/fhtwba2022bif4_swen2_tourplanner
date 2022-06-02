using System.Text.RegularExpressions;
using TourPlanner.Common.DTO;
using TourPlanner.Common.Logging;
using TourPlanner.Common.PDF;
using TourPlanner.DAL;
using TourPlanner.DAL.MapQuest;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.BL
{
    public class TourController
    {
        protected static ILoggerWrapper logger = LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static readonly TourRepository tourRepository = new TourRepository(DbContext.GetInstance());

        public static IEnumerable<TourDto> GetItems(string filter)
        {
            if (filter == null || filter == string.Empty)
                return tourRepository.Get();
            else
                return tourRepository.Get().ToList().Where(t => Regex.Match(t.ToJson() + string.Join("", TourLogController.GetLogsOfTour(t.Id)), filter, RegexOptions.IgnoreCase).Success);
        }

        public static TourDto GetById(Guid id)
        {
            return tourRepository.GetById(id);
        }

        public static bool AddItem(TourDto tourDto)
        {
            return tourRepository.Insert(tourDto);
        }

        public static async Task<bool> UpdateItem(TourDto tour)
        {
            var metaData = await MapQuestService.GetRouteMetaData(tour.From, tour.To, tour.TransportType);

            if (metaData == null) return false;

            tour.Distance = metaData?.Distance ?? Double.NegativeInfinity;
            tour.EstimatedTime = metaData?.FormattedTime ?? TimeSpan.Zero;

            await GetRouteImage(tour);

            return tourRepository.Update(tour);
        }

        public static bool RemoveItem(TourDto tourDto)
        {
            return tourRepository.Delete(tourDto.Id);
        }

        public static async Task<bool> GetRouteImage(TourDto tour)
        {
            return await MapQuestService.GetRouteImage(tour.Id.ToString(), tour.From, tour.To);
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
