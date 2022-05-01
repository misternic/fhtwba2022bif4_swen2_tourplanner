using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TourPlanner.Common;
using TourPlanner.DAL;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.BL
{
    public class TourLogController
    {
        static readonly BaseRepository<TourLog> tourLogRepository = new TourLogRepository(DbContext.GetInstance());

        public static IEnumerable<TourLog> GetLogsOfTour(Guid id)
        {
            return tourLogRepository.Get().Where(l => l.TourId == id);
        }
    }
}
