using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.BL;
using TourPlanner.DAL;
using TourPlanner.ViewModels;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            var context = DbContext.GetInstance();
            context.Init();

            var menuViewModel = new MenuViewModel();
            var searchViewModel = new SearchViewModel();
            var toursViewModel = new ToursViewModel();
            var tourDetailsViewModel = new TourDetailsViewModel();
            var tourLogsViewModel = new TourLogsViewModel();

            var tourController = new TourController();
            var tourLogController = new TourLogController();


            var wnd = new MainWindow()
            {
                DataContext = new MainViewModel(menuViewModel, searchViewModel, toursViewModel, tourDetailsViewModel, tourLogsViewModel, tourController, tourLogController),

                Menu = { DataContext = menuViewModel },
                Search = { DataContext = searchViewModel },
                Tours = { DataContext = toursViewModel },
                TourDetails = { DataContext = tourDetailsViewModel },
                TourLogs = { DataContext = tourLogsViewModel }
            };

            wnd.Show();
        }
    }
}
