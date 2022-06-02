using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TourPlanner.Common;
using TourPlanner.Common.DTO;
using TourPlanner.DAL;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.BL
{
    public class TourLogController
    {
        static readonly BaseRepository<TourLogDto> tourLogRepository = new TourLogRepository(DbContext.GetInstance());

        public static IEnumerable<TourLogDto> GetLogsOfTour(Guid id)
        {
            return tourLogRepository.Get().Where(l => l.TourId == id);
        }

        public bool AddTourLog(TourLogDto tourLog)
        {
            return tourLogRepository.Insert(tourLog);
        }
    }
}
