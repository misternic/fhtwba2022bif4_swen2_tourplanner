using System.Collections.Generic;
using TourPlanner.Common;

namespace TourPlanner.BL
{
    public interface ITourFactory
    {
        IEnumerable<Tour> GetItems(string filter);
        bool AddItem(Tour tour);
        bool RemoveItem(Tour tour);
    }
}
