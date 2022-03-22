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

            var menuViewModel = new MenuViewModel();
            var searchbarViewModel = new SearchbarViewModel();
            var sidebarViewModel = new SidebarViewModel();
            var tourlogsViewModel = new TourLogsViewModel();
            var tourViewModel = new TourViewModel();

            var wnd = new MainWindow()
            {
                DataContext = new MainViewModel(menuViewModel, searchbarViewModel, sidebarViewModel, tourlogsViewModel, tourViewModel),

                Menu = { DataContext = menuViewModel },
                Searchbar = { DataContext = searchbarViewModel },
                Sidebar = { DataContext = sidebarViewModel },
                TourLogs = { DataContext = tourlogsViewModel },
                Tour = { DataContext = tourViewModel }
            };

            wnd.Show();
        }
    }
}
