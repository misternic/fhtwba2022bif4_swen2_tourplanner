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
        
        public IEnumerable<Tour> GetItems()
        {
            return _tourRepository.Get();
        }

        public IEnumerable<Tour> Search(string searchText)
        {
            return _tourRepository.Get().ToList().Where(t => Regex.Match(t.ToJson(), searchText, RegexOptions.IgnoreCase).Success);
        }
    }
}
