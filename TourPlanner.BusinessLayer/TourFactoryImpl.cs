using System;
using System.Collections.Generic;

namespace TourPlanner.BusinessLayer
{
    internal class TourFactoryImpl : ITourFactory
    {
        public IEnumerable<Tour> GetItems()
        {
            return new List<Tour>()
            {
                new Tour() { Name = "Tour Item 1" },
                new Tour() { Name = "Tour Item 2" },
                new Tour() { Name = "Tour Item 3" },
                new Tour() { Name = "Tour Item 4" }
            };
        }

        public IEnumerable<Tour> Search(string name)
        {
            throw new NotImplementedException();
        }
    }
}
