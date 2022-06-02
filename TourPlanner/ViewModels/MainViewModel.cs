using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TourPlanner.BL;
using TourPlanner.Common.DTO;
using TourPlanner.Common.Logging;
using TourPlanner.DAL.MapQuest;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        protected static ILoggerWrapper logger = LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private MenuViewModel menuViewModel;
        private SearchViewModel searchViewModel;
        private ToursViewModel toursViewModel;
        private TourDetailsViewModel tourDetailsViewModel;
        private TourLogsViewModel tourLogsViewModel;

        public MainViewModel(MenuViewModel menuViewModel, SearchViewModel searchViewModel, ToursViewModel toursViewModel, TourDetailsViewModel tourDetailsViewModel, TourLogsViewModel tourLogsViewModel)
        {
            this.menuViewModel = menuViewModel;
            this.searchViewModel = searchViewModel;
            this.toursViewModel = toursViewModel;
            this.tourDetailsViewModel = tourDetailsViewModel;
            this.tourLogsViewModel = tourLogsViewModel;

            this.SetupUI();
        }

        public void SetupUI ()
        {
            menuViewModel.AddEvent += (_, e) => this.AddTour();
            menuViewModel.ExportEvent += (_, e) => this.ExportData();
            menuViewModel.ImportEvent += (_, e) => this.ImportData();
            menuViewModel.SummaryExportAsPdfEvent += (_, e) => this.ExportSummaryAsPdf();

            menuViewModel.SaveEvent += (_, e) => this.SaveTour(this.toursViewModel.SelectedTour);
            menuViewModel.DeleteEvent += (_, e) => this.RemoveTour(this.toursViewModel.SelectedTour);
            menuViewModel.ExportAsPdfEvent += (_, tour) => this.ExportTourAsPdf(this.toursViewModel.SelectedTour, this.tourLogsViewModel.TourLogs.ToList());

            searchViewModel.SearchEvent += (_, filter) => this.LoadTours(filter);
            toursViewModel.AddEvent += (_, e) => this.AddTour();
            tourDetailsViewModel.SaveEvent += (_, tour) => this.SaveTour(tour);
            toursViewModel.RemoveEvent += (_, tour) => this.RemoveTour(tour);
            toursViewModel.SelectedEvent += (_, tour) => this.TourSelected(tour);

            this.LoadTours();
        }
        public void LoadTours(string filter = null)
        {
            toursViewModel.Tours.Clear();

            foreach (TourDto item in TourController.GetItems(filter))
            {
                toursViewModel.Tours.Add(item);
            }
        }

        public void ClearFilter()
        {
            this.searchViewModel.SearchText = String.Empty;
        }

        public void TourSelected(TourDto tour)
        {
            var tourDB = TourController.GetById(tour.Id);

            logger.Debug($"Tour selected: {tourDB.Id} {tourDB.Name}");
            logger.Debug(tourDB.ToJson());

            menuViewModel.TourIsSelected = true;
            tourDetailsViewModel.Visibility = Visibility.Visible;
            tourLogsViewModel.Visibility = Visibility.Visible;

            tourDetailsViewModel.Tour = tourDB;
            tourLogsViewModel.LoadTourLogs(TourLogController.GetLogsOfTour(tourDB.Id));
        }

        public void TourDeselected()
        {
            menuViewModel.TourIsSelected = false;
            tourDetailsViewModel.Visibility = Visibility.Hidden;
            tourLogsViewModel.Visibility = Visibility.Hidden;
        }

        public void AddTour()
        {
            Guid newId = Guid.NewGuid();

            TourDto t = new TourDto()
            {
                Id = newId,
                Name = "New Tour",
                Description = "",
                From = "",
                To = ""
            };
            TourController.AddItem(t);

            ClearFilter();
            LoadTours();
            toursViewModel.SelectedTour = this.toursViewModel.Tours.Where(t => t.Id == newId).Single();
        }

        public void RemoveTour(TourDto tour)
        {
            MessageBoxResult result = MessageBox.Show("Click yes if you want to delete the tour.", "TourPlanner", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TourController.RemoveItem(tour);
                this.LoadTours(this.searchViewModel.SearchText);
                this.TourDeselected();
            }
        }

        public async void SaveTour(TourDto tour)
        {
            var metaData = await MapQuestService.GetRouteMetaData(tour.From, tour.To, tour.TransportType);




            if (metaData != null)
            {
                tour.Distance = metaData?.Distance ?? Double.NegativeInfinity;
                tour.EstimatedTime = metaData?.FormattedTime ?? TimeSpan.Zero;

                TourController.UpdateItem(tour);
                LoadTours();
                TourSelected(tour);
                await tourDetailsViewModel.LoadRouteImageAsync();
                toursViewModel.SelectedTour = this.toursViewModel.Tours.Where(t => t.Id == tour.Id).Single();
            }
        }

        public void ExportData()
        {
            logger.Info($"Export data in the database...");

            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = "data",
                Filter = "JSON Document (*.json)|*.json",
                InitialDirectory = Config["PersistenceFolder"]
            };


            if (dialog.ShowDialog() == true)
            {
                logger.Info($"Filepath: {dialog.FileName}");

                if (TourController.ExportData(dialog.FileName))
                    MessageBox.Show("Export of data succeeded.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Export of data failed.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ImportData()
        {
            logger.Info($"Import data in the database...");

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "JSON Document (*.json)|*.json",
                InitialDirectory = Config["PersistenceFolder"]
            };

            if (dialog.ShowDialog() == true)
            {
                logger.Info($"Filepath: {dialog.FileName}");

                if (TourController.ImportData(dialog.FileName))
                    MessageBox.Show("Import of data succeeded.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Import of data failed.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExportTourAsPdf(TourDto tour, List<TourLogDto> logs)
        {
            logger.Info($"Export tour as PDF");

            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = tour.Name,
                Filter = "PDF Document (*.pdf)|*.pdf",
                InitialDirectory = Config["PersistenceFolder"]
            };

            if (dialog.ShowDialog() == true)
            {
                logger.Info($"Save as {dialog.FileName}");

                if (TourController.ExportTourAsPdf(dialog.FileName, tour, logs))
                    MessageBox.Show("Export of tour succeeded.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Export of tour failed.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ExportSummaryAsPdf()
        {
            logger.Info($"Summary export as PDF");

            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = "summary",
                Filter = "PDF Document (*.pdf)|*.pdf",
                InitialDirectory = Config["PersistenceFolder"]
            };

            if (dialog.ShowDialog() == true)
            {
                logger.Info($"Save as {dialog.FileName}");

                if (TourController.ExportSummaryAsPdf(dialog.FileName))
                    MessageBox.Show("Export of summary succeeded.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Export of summary failed.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
