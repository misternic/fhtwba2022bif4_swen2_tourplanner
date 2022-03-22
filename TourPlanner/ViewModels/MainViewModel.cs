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
        private ITourFactory _tourFactory;

        public ObservableCollection<Tour> Tours { get; set; } = new ObservableCollection<Tour>();

        public MainViewModel(MenuViewModel menu, SearchbarViewModel searchbar, SidebarViewModel sidebar, TourLogsViewModel tourLogs, TourViewModel tour)
        {            
            this._tourFactory = TourFactory.GetInstance();

            this.LoadAllItems();

            searchbar.SearchEvent += (_, searchText) =>
            {
                this.Search(searchText);
            };
        }

        public void LoadAllItems()
        {
            foreach (Tour item in this._tourFactory.GetItems())
            {
                this.Tours.Add(item);
            }
        }

        public void Search(string searchText)
        {
            this.Tours.Clear();

            foreach (Tour item in this._tourFactory.Search(searchText))
            {
                this.Tours.Add(item);
            }
        }
    }
}
