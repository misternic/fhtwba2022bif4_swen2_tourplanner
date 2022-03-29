using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        private MenuViewModel menuVM;
        private SearchViewModel searchVM;
        private ToursViewModel toursVM;
        private TourDetailsViewModel tourDetailsVM;
        private TourLogsViewModel tourLogsVM;

        public MainViewModel(MenuViewModel menuVM, SearchViewModel searchVM, ToursViewModel toursVM, TourDetailsViewModel tourDetailsVM, TourLogsViewModel tourLogsVM)
        {
            this.menuVM = menuVM;
            this.searchVM = searchVM;
            this.toursVM = toursVM;
            this.tourDetailsVM = tourDetailsVM;
            this.tourLogsVM = tourLogsVM;

            this.SetupUI();
        }

        public void SetupUI ()
        {
            this.LoadTours();

            this.toursVM.AddEvent += (_, e) => this.AddTour();
            this.toursVM.RemoveEvent += (_, tour) => this.RemoveTour(tour);
            this.searchVM.SearchEvent += (_, filter) => this.LoadTours(filter);
            this.toursVM.SelectedEvent += (_, tour) => this.LoadTourLogs(tour);
        }
        public void LoadTours(string filter = null)
        {
            this.toursVM.Tours.Clear();

            foreach (Tour item in TourController.GetItems(filter))
            {
                this.toursVM.Tours.Add(item);
            }
        }

        public void ClearFilter()
        {
            this.searchVM.SearchText = "";
        }

        public void TourSelected(Tour tour)
        {
            //this.LoadTourDetails(tour); TODO
            this.LoadTourLogs(tour);
        }

        public void LoadTourLogs(Tour tour)
        {
            //this.tourLogs.TourLog = this._tourFactory.LoadTourLogs(tour); TODO
        }

        public void AddTour()
        {
            Guid newId = Guid.NewGuid();

            Tour t = new Tour();
            t.Id = newId;
            t.Name = "New Tour";
            t.Description = "";
            t.From = "";
            t.To = "";
            TourController.AddItem(t);

            this.ClearFilter();
            this.LoadTours();
            this.toursVM.SelectedTour = this.toursVM.Tours.Where(t => t.Id == newId).Single();
        }

        public void RemoveTour(Tour tour)
        {
            TourController.RemoveItem(tour);
            this.LoadTours(this.searchVM.SearchText);
        }
    }
}
