using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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

            var menuVM = new MenuViewModel();
            var searchVM = new SearchViewModel();
            var toursVM = new ToursViewModel();
            var tourDetailsVM = new TourDetailsViewModel();
            var tourLogsVM = new TourLogsViewModel();

            var wnd = new MainWindow()
            {
                DataContext = new MainViewModel(menuVM, searchVM, toursVM, tourDetailsVM, tourLogsVM),

                Menu = { DataContext = menuVM },
                Search = { DataContext = searchVM },
                Tours = { DataContext = toursVM },
                TourDetails = { DataContext = tourDetailsVM },
                TourLogs = { DataContext = tourLogsVM }
            };

            wnd.Show();
        }
    }
}
