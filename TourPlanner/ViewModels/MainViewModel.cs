using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BL;
using TourPlanner.Common;
using TourPlanner.DAL;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ITourFactory tourFactory;

        public ObservableCollection<Tour> Tours { get; set; }

        public MainViewModel()
        {
            var context = DbContext.GetInstance();
            context.Init();
            
            tourFactory = TourFactory.GetInstance();

            Tours = new ObservableCollection<Tour>();

            foreach(Tour item in this.tourFactory.GetItems())
            {
                Tours.Add(item);
            }
        }
    }
}
