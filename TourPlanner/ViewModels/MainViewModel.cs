using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BusinessLayer;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ITourFactory tourFactory;

        public ObservableCollection<Tour> Tours { get; set; }

        public MainViewModel()
        {
            this.tourFactory = TourFactory.GetInstance(); 

            Tours = new ObservableCollection<Tour>();

            foreach(Tour item in this.tourFactory.GetItems())
            {
                Tours.Add(item);
            }
        }
    }
}
