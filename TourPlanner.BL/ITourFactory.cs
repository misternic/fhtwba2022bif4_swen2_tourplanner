using System.Collections.Generic;
using TourPlanner.Common;

namespace TourPlanner.BL
{
    public interface ITourFactory
    {
        IEnumerable<Tour> GetItems();
        IEnumerable<Tour> Search(string searchText);
    }
}
