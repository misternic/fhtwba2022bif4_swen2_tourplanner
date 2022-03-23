using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using TourPlanner.Common;
using TourPlanner.DAL;
using TourPlanner.DAL.Repositories;

namespace TourPlanner.BL
{
    internal class TourFactoryImpl : ITourFactory
    {
        private readonly BaseRepository<Tour> _tourRepository;

        public TourFactoryImpl()
        {
            var context = DbContext.GetInstance();
            _tourRepository = new TourRepository(context);
        }

        public IEnumerable<Tour> GetItems(string filter)
        {
            if (filter == null || filter == string.Empty)
                return _tourRepository.Get();
            else
                return _tourRepository.Get().ToList().Where(t => Regex.Match(t.ToJson(), filter, RegexOptions.IgnoreCase).Success);
        }

        public bool AddItem(Tour tour)
        {
            return this._tourRepository.Insert(tour);
        }

        public bool RemoveItem(Tour tour)
        {
            return this._tourRepository.Delete(tour.Id);
        }
    }
}
