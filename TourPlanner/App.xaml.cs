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

            var searchBarViewModel = new SearchBarViewModel();

            var wnd = new MainWindow
            {
                DataContext = new MainViewModel(searchBarViewModel),
                SearchBar = { DataContext = searchBarViewModel }
            };

            wnd.Show();
        }
    }
}
