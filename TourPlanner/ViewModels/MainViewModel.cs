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
        private MenuViewModel menu;
        private SearchbarViewModel searchbar;
        private SidebarViewModel sidebar;
        private TourLogsViewModel tourLogs;
        private TourViewModel tour;

        public MainViewModel(MenuViewModel menu, SearchbarViewModel searchbar, SidebarViewModel sidebar, TourLogsViewModel tourLogs, TourViewModel tour)
        {
            this.menu = menu;
            this.searchbar = searchbar;
            this.sidebar = sidebar;
            this.tourLogs = tourLogs;
            this.tour = tour;

            this.SetupUI();
        }

        public void SetupUI ()
        {
            this.LoadTours();

            sidebar.AddEvent += (_, tour) => this.AddTour(tour);
            sidebar.RemoveEvent += (_, tour) => this.RemoveTour(tour);
            searchbar.SearchEvent += (_, filter) => this.LoadTours(filter);
            sidebar.SelectedEvent += (_, tour) => this.LoadTourLogs(tour);
        }
        public void LoadTours(string filter = null)
        {
            this.sidebar.Tours.Clear();

            foreach (Tour item in TourController.GetItems(filter))
            {
                this.sidebar.Tours.Add(item);
            }
        }

        public void ClearFilter()
        {
            this.searchbar.SearchText = "";
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

        public void AddTour(Tour tour)
        {
            tour.Id = Guid.NewGuid();
            tour.Name = "New Tour";
            tour.Description = "";
            tour.From = "";
            tour.To = "";
            TourController.AddItem(tour);

            this.ClearFilter();
            this.LoadTours();
            // sidebar.SelectedTour = tour; TODO
        }

        public void RemoveTour(Tour tour)
        {
            TourController.RemoveItem(tour);
            this.LoadTours(this.searchbar.SearchText);
        }
    }
}
