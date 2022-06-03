using System.Text.RegularExpressions;
using TourPlanner.Common.DTO;
using TourPlanner.Common.Logging;
using TourPlanner.Common.PDF;
using TourPlanner.DAL;
using TourPlanner.DAL.MapQuest;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.BL
{
    public class TourController : ITourController
    {
        protected static ILoggerWrapper logger = LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        static readonly TourRepository tourRepository = new TourRepository(DbContext.GetInstance());
        static readonly TourLogRepository tourLogRepository = new TourLogRepository(DbContext.GetInstance());

        public IEnumerable<TourDto> GetItems(string filter)
        {
            if (filter == null || filter == string.Empty)
                return tourRepository.Get();
            else
                return tourRepository.Get().ToList().Where(t => Regex.Match(t.ToJson() + string.Join("", GetLogsOfTour(t.Id)), filter, RegexOptions.IgnoreCase).Success);
        }

        public TourDto GetById(Guid id)
        {
            return tourRepository.GetById(id);
        }

        public IEnumerable<TourLogDto> GetLogsOfTour(Guid id)
        {
            return tourLogRepository.Get().Where(log => log.TourId.Equals(id)).ToList();
        }

        public bool AddItem(TourDto tourDto)
        {
            return tourRepository.Insert(tourDto);
        }

        public async Task<bool> UpdateItem(TourDto tour)
        {
            tour.Name = tour.Name.Trim();
            tour.Description = tour.Description.Trim();
            tour.To = tour.To.Trim();
            tour.From = tour.From.Trim();

            if (tour.Name == String.Empty || tour.Description == String.Empty || tour.To == String.Empty || tour.From == String.Empty)
                return false;

            var metaData = await MapQuestService.GetRouteMetaData(tour.From, tour.To, tour.TransportType);

            if (metaData == null) return false;

            tour.Distance = metaData?.Distance ?? Double.NegativeInfinity;
            tour.EstimatedTime = metaData?.FormattedTime ?? TimeSpan.Zero;

            _ = GetRouteImage(tour);

            return tourRepository.Update(tour);
        }

        public bool RemoveItem(TourDto tourDto)
        {
            return tourRepository.Delete(tourDto.Id);
        }

        public async Task<bool> GetRouteImage(TourDto tour)
        {
            return await MapQuestService.GetRouteImage(tour.Id.ToString(), tour.From, tour.To);
        }

        public bool ExportData(string path)
        {
            // TODO: get data from database, serialize, and write to file
            return true;
        }

        public bool ImportData(string path)
        {
            // TODO: get json from file, deserialize and send to database
            return true;
        }

        public bool ExportTourAsPdf(string path, TourDto tour, List<TourLogDto> logs)
        {
            var report = new TourReport(tour, logs);
            return report.ExportToPdf(path);
        }

        public bool ExportSummaryAsPdf(string path)
        {
            var report = new SummaryReport((ICollection<TourSummaryDto>)tourRepository.GetTourSummaries());
            return report.ExportToPdf(path);
        }
    }
}
