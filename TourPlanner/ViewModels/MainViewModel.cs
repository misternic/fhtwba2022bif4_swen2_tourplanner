using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TourPlanner.BL;
using TourPlanner.Common.DTO;
using TourPlanner.Common.Logging;
using TourPlanner.ViewModels.Abstract;
using TourPlanner.Views;

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

        private ITourController tourController;
        private ITourLogController tourLogController;

        public MainViewModel(MenuViewModel menuViewModel, SearchViewModel searchViewModel, ToursViewModel toursViewModel, TourDetailsViewModel tourDetailsViewModel, TourLogsViewModel tourLogsViewModel, ITourController tourController, ITourLogController tourLogController)
        {
            this.menuViewModel = menuViewModel;
            this.searchViewModel = searchViewModel;
            this.toursViewModel = toursViewModel;
            this.tourDetailsViewModel = tourDetailsViewModel;
            this.tourLogsViewModel = tourLogsViewModel;

            this.tourController = tourController;
            this.tourLogController = tourLogController;

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
            toursViewModel.DeleteEvent += (_, tour) => this.RemoveTour(tour);
            toursViewModel.SelectedEvent += (_, tour) => this.TourSelected(tour);
            toursViewModel.ReloadEvent += (_, e) => this.LoadTours();

            tourDetailsViewModel.SaveEvent += (_, tour) => this.SaveTour(tour);
            tourDetailsViewModel.DeleteEvent += (_, tour) => this.RemoveTour(tour);
            tourDetailsViewModel.ExportAsPdfEvent += (_, tour) => this.ExportTourAsPdf(this.toursViewModel.SelectedTour, this.tourLogsViewModel.TourLogs.ToList());

            tourLogsViewModel.AddEvent += (_, e) => this.AddTourLog();
            tourLogsViewModel.DeleteEvent += (_, tourLog) => this.DeleteTourLog(tourLog);
            tourLogsViewModel.EditEvent += (_, tourLog) => this.EditTourLog(tourLog);

            this.LoadTours();
        }
        public void LoadTours(string filter = null)
        {
            toursViewModel.LoadTours(tourController.GetItems(filter));
        }

        public void ClearFilter()
        {
            this.searchViewModel.SearchText = String.Empty;
        }

        public void TourSelected(TourDto tour)
        {
            logger.Debug($"Tour selected: {tour.Id} {tour.Name}");

            menuViewModel.TourIsSelected = true;

            LoadTourDetails(tour.Id);
            LoadTourLogs(tour.Id);
        }

        public void LoadTourDetails(Guid id)
        {
            var tour = tourController.GetById(id);
            tourDetailsViewModel.Tour = tour;
            tourDetailsViewModel.Visibility = Visibility.Visible;
        }

        public void LoadTourLogs(Guid id)
        {
            var tourLogs = tourLogController.GetLogsOfTour(id);
            tourLogsViewModel.LoadTourLogs(tourLogs);
            tourLogsViewModel.Visibility = Visibility.Visible;
        }

        public void TourDeselected()
        {
            menuViewModel.TourIsSelected = false;
            tourDetailsViewModel.Visibility = Visibility.Hidden;
            tourLogsViewModel.Visibility = Visibility.Hidden;
        }

        public void AddTour()
        {
            TourDto newtour = new TourDto()
            {
                Id = Guid.NewGuid(),
                Name = "New Tour",
                Description = "",
                From = "",
                To = ""
            };
            tourController.AddItem(newtour);

            ClearFilter();
            LoadTours();
            LoadTourDetails(newtour.Id);
            LoadTourLogs(newtour.Id);
            toursViewModel.SelectTourWithoutEvent(newtour.Id);
        }

        public async void RemoveTour(TourDto tour)
        {
            object dialogResult = await DialogHost.Show(new TextBlock { Text = "Click yes if you want to delete the tour" }, "YesNoDialog");

            if (dialogResult is bool boolResult && boolResult)
            {
                tourController.RemoveItem(tour);
                this.LoadTours(this.searchViewModel.SearchText);
                this.TourDeselected();
            }
        }

        public async void SaveTour(TourDto tour)
        {
            if (await tourController.UpdateItem(tour))
            {
                MessageBox.Show("Tour saved.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadTours();
                LoadTourDetails(tour.Id);
                toursViewModel.SelectTourWithoutEvent(tour.Id);
            }
            else
                MessageBox.Show("Saving tour failed.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public async void AddTourLog()
        {
            Guid newId = Guid.NewGuid();

            TourLogDto newTourLog = new TourLogDto()
            {
                Id = newId,
                TourId = toursViewModel.SelectedTour.Id,
                Date = new DateOnly(2022, 06, 02),
                Difficulty = Common.Difficulty.Medium,
                Comment = "Test"
            };

            await tourLogController.AddTourLogAsync(newTourLog);
            LoadTourDetails(toursViewModel.SelectedTour.Id);
            LoadTourLogs(toursViewModel.SelectedTour.Id);
        }

        public void DeleteTourLog(TourLogDto tourLog)
        {
            tourLogController.DeleteTourLog(tourLog);
            LoadTourDetails(toursViewModel.SelectedTour.Id);
            LoadTourLogs(toursViewModel.SelectedTour.Id);
        }

        public async void EditTourLog(TourLogDto tourLog)
        {
            await DialogHost.Show(tourLog, "TourLogDialog");
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

                if (tourController.ExportData(dialog.FileName))
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

                if (tourController.ImportData(dialog.FileName))
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

                if (tourController.ExportTourAsPdf(dialog.FileName, tour, logs))
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

                if (tourController.ExportSummaryAsPdf(dialog.FileName))
                    MessageBox.Show("Export of summary succeeded.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Information);
                else
                    MessageBox.Show("Export of summary failed.", "TourPlanner", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
