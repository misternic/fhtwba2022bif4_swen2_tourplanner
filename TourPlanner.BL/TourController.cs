using Newtonsoft.Json;
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

        private readonly ITourRepository tourRepository;
        private readonly IRepository<TourLogDto> tourLogRepository;

        public TourController() : this(new TourRepository(DbContext.GetInstance()), new TourLogRepository(DbContext.GetInstance())) { }

        public TourController(ITourRepository tourRepository, IRepository<TourLogDto> tourLogRepository)
        {
            this.tourRepository = tourRepository;
            this.tourLogRepository = tourLogRepository;
        }

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

        public async Task<bool> ExportData(string path)
        {
            var data = new ExportDto()
            {
                Tours = tourRepository.Get(),
                Logs = tourLogRepository.Get()
            };

            try
            {
                await File.WriteAllTextAsync(path, data.ToJson());
            }
            catch(Exception e)
            {
                return false;
            }

            return true;
        }

        public bool ImportData(string path)
        {
            logger.Info($"Start import data from {path}...");
            var data = JsonConvert.DeserializeObject<ExportDto>(File.ReadAllText(path));

            if (data != null && data.Tours != null)
            {
                tourRepository.Get().ToList().ForEach(t => tourRepository.Delete(t.Id));
                tourLogRepository.Get().ToList().ForEach(l => tourLogRepository.Delete(l.Id));

                data.Tours.ToList().ForEach(t => tourRepository.Insert(t));
                data.Logs.ToList().ForEach(l => tourLogRepository.Insert(l));

                return true;
            }

            logger.Info($"Import failed because schema of json file isn't suitable.");

            return false;
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
