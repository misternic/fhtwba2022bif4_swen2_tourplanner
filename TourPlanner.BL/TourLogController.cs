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
    public class TourLogController
    {
        static readonly BaseRepository<TourLogDto> tourLogRepository = new TourLogRepository(DbContext.GetInstance());

        public static IEnumerable<TourLogDto> GetLogsOfTour(Guid id)
        {
            return tourLogRepository.Get().Where(l => l.TourId == id);
        }

        public static async Task<bool> AddTourLogAsync(TourLogDto tourLog)
        {
            var tour = TourController.GetById(tourLog.TourId);

            if (!String.IsNullOrWhiteSpace(tour.To))
                tourLog.Temperature = await WeatherApiService.GetTemperatureAtDateAsync(tour.To, tourLog.Date);

            return tourLogRepository.Insert(tourLog);
        }
        public static bool UpdateTourLog(TourLogDto tourLog)
        {
            return tourLogRepository.Update(tourLog);
        }

        public static bool DeleteTourLog(TourLogDto tourLog)
        {
            return tourLogRepository.Delete(tourLog.Id);
        }
    }
}
