using System;
using System.Collections.Generic;
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

        public IEnumerable<Tour> Search(string name)
        {
            throw new NotImplementedException();
        }
    }
}
