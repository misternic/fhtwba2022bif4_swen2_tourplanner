using System.Collections.Generic;
using TourPlanner.Common;

namespace TourPlanner.BL
{
    public interface ITourFactory
    {
        IEnumerable<Tour> GetItems();
        bool AddItem(Tour tour);
        bool RemoveItem(Tour tour);
        IEnumerable<Tour> Search(string searchText);
    }
}
