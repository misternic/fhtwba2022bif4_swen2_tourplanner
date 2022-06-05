using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TourPlanner.Common;
using TourPlanner.Common.DTO;
using TourPlanner.DAL;
using TourPlanner.DAL.Repositories;
using TourPlanner.DAL.WeatherApi;

namespace TourPlanner.BL
{
    public class TourLogController : ITourLogController
    {
        private readonly ITourRepository tourRepository;
        private readonly IRepository<TourLogDto> tourLogRepository;

        public TourLogController() : this(new TourRepository(DbContext.GetInstance()), new TourLogRepository(DbContext.GetInstance())) { }

        public TourLogController(ITourRepository tourRepository, IRepository<TourLogDto> tourLogRepository)
        {
            this.tourRepository = tourRepository;
            this.tourLogRepository = tourLogRepository;
        }

        public async Task<bool> Save(TourLogDto tourLog)
        {
            if (tourLog == null || tourLog.Date == null || tourLog.Duration == null || tourLog.Difficulty == null) return false;

            var tour = tourRepository.GetById(tourLog.TourId);

            if (!String.IsNullOrWhiteSpace(tour.To))
                tourLog.Temperature = await WeatherApiService.GetTemperatureAtDateAsync(tour.To, tourLog.Date);

            if (tourLogRepository.GetById(tourLog.Id) == null)
                return tourLogRepository.Insert(tourLog);
            else
                return tourLogRepository.Update(tourLog);
        }


        public IEnumerable<TourLogDto> GetLogsOfTour(Guid id)
        {
            return tourLogRepository.Get().Where(l => l.TourId == id);
        }

        public bool DeleteTourLog(TourLogDto tourLog)
        {
            return tourLogRepository.Delete(tourLog.Id);
        }

        public TourLogDto GetById(Guid id)
        {
            return tourLogRepository.GetById(id);
        }
    }
}
