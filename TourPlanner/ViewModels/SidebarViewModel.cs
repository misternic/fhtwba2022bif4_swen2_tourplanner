using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Common;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class SidebarViewModel : BaseViewModel
    {
        public ObservableCollection<Tour> Tours { get; set; } = new ObservableCollection<Tour>();

    }
}
