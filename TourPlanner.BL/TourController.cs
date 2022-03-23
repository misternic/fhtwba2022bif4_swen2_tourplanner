using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TourPlanner.Common;
using TourPlanner.DAL;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.BL
{
    public class TourController
    {
        static readonly BaseRepository<Tour> tourRepository = new TourRepository(DbContext.GetInstance());

        public static IEnumerable<Tour> GetItems(string filter)
        {
            if (filter == null || filter == string.Empty)
                return tourRepository.Get();
            else
                return tourRepository.Get().ToList().Where(t => Regex.Match(t.ToJson(), filter, RegexOptions.IgnoreCase).Success);
        }

        public static bool AddItem(Tour tour)
        {
            return tourRepository.Insert(tour);
        }

        public static bool RemoveItem(Tour tour)
        {
            return tourRepository.Delete(tour.Id);
        }
    }
}
