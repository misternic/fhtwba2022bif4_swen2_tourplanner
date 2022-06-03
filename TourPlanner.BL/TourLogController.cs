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
        static readonly BaseRepository<TourDto> tourRepository = new TourRepository(DbContext.GetInstance());
        static readonly BaseRepository<TourLogDto> tourLogRepository = new TourLogRepository(DbContext.GetInstance());

        public IEnumerable<TourLogDto> GetLogsOfTour(Guid id)
        {
            return tourLogRepository.Get().Where(l => l.TourId == id);
        }

        public async Task<bool> AddTourLogAsync(TourLogDto tourLog)
        {
            var tour = tourRepository.GetById(tourLog.TourId);

            if (!String.IsNullOrWhiteSpace(tour.To))
                tourLog.Temperature = await WeatherApiService.GetTemperatureAtDateAsync(tour.To, tourLog.Date);

            return tourLogRepository.Insert(tourLog);
        }
        public bool UpdateTourLog(TourLogDto tourLog)
        {
            return tourLogRepository.Update(tourLog);
        }

        public bool DeleteTourLog(TourLogDto tourLog)
        {
            return tourLogRepository.Delete(tourLog.Id);
        }
    }
}
