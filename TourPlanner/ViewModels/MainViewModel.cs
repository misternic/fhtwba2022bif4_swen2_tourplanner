using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlanner.BL;
using TourPlanner.Common;
using TourPlanner.Common.DTO;
using TourPlanner.Common.PDF;
using TourPlanner.DAL;
using TourPlanner.DAL.MapQuest;
using TourPlanner.DAL.Repositories;
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
            this.menuVM.AddEvent += (_, e) => this.AddTour();
            this.menuVM.ExportEvent += (_, e) => this.ExportData();
            this.menuVM.ImportEvent += (_, e) => this.ImportData();
            this.menuVM.SummaryExportAsPdfEvent += (_, e) => this.ExportSummaryAsPdf();

            this.menuVM.SaveEvent += (_, e) => this.SaveTour(this.toursVM.SelectedTour);
            this.menuVM.DeleteEvent += (_, e) => this.RemoveTour(this.toursVM.SelectedTour);
            this.menuVM.ExportAsPdfEvent += (_, tour) => this.ExportTourAsPdf(this.toursVM.SelectedTour, this.tourLogsVM.TourLogs.ToList());

            this.searchVM.SearchEvent += (_, filter) => this.LoadTours(filter);
            this.toursVM.AddEvent += (_, e) => this.AddTour();
            this.tourDetailsVM.SaveEvent += (_, tour) => this.SaveTour(tour);
            this.toursVM.RemoveEvent += (_, tour) => this.RemoveTour(tour);
            this.toursVM.SelectedEvent += (_, tour) => this.TourSelected(tour);

            this.LoadTours();
        }
        public void LoadTours(string filter = null)
        {
            this.toursVM.Tours.Clear();

            foreach (TourDto item in TourController.GetItems(filter))
            {
                this.toursVM.Tours.Add(item);
            }
        }

        public void ClearFilter()
        {
            this.searchVM.SearchText = String.Empty;
        }

        public void TourSelected(TourDto tourDto)
        {
            Debug.Print($"Tour selected: {tourDto.Id} {tourDto.Name}");
            Debug.Print(tourDto.ToJson());

            this.menuVM.TourIsSelected = true;
            this.LoadTourDetails(tourDto);
            this.LoadTourLogs(tourDto);
        }

        public void TourDeselected()
        {
            this.menuVM.TourIsSelected = false;
        }

        public void LoadTourDetails(TourDto tourDto)
        {
            tourDetailsVM.Tour = tourDto;
        }

        public void LoadTourLogs(TourDto tourDto)
        {
            tourLogsVM.TourLogs.Clear();

            foreach (TourLogDto item in TourLogController.GetLogsOfTour(tourDto.Id))
            {
                this.tourLogsVM.TourLogs.Add(item);
            }
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

            this.ClearFilter();
            this.LoadTours();
            this.toursVM.SelectedTour = this.toursVM.Tours.Where(t => t.Id == newId).Single();
        }

        public void RemoveTour(TourDto tour)
        {
            MessageBoxResult result = MessageBox.Show("Click yes if you want to delete the tour.", "TourPlanner", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                TourController.RemoveItem(tour);
                this.LoadTours(this.searchVM.SearchText);
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

                this.TourSelected(tour);
                TourController.UpdateItem(tour);
                tourDetailsVM.LoadRouteImageAsync();
            }
        }

        public void ExportData()
        {
            Debug.Print($"Export data in the database...");

            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = "data",
                Filter = "JSON Document (*.json)|*.json",
                InitialDirectory = Config["PersistenceFolder"]
            };


            if (dialog.ShowDialog() == true)
            {
                Debug.Print($"Filepath: {dialog.FileName}");
                if (TourController.ExportData(dialog.FileName))
                {
                    Debug.Print($"succeeded");
                }
                else
                {
                    Debug.Print($"failed");
                }
            }
        }

        public void ImportData()
        {
            Debug.Print($"Import data in the database...");

            OpenFileDialog dialog = new OpenFileDialog()
            {
                Filter = "JSON Document (*.json)|*.json",
                InitialDirectory = Config["PersistenceFolder"]
            };

            if (dialog.ShowDialog() == true)
            {
                Debug.Print($"Filepath: {dialog.FileName}");
                if (TourController.ImportData(dialog.FileName))
                {
                    Debug.Print($"succeeded");
                }
                else
                {
                    Debug.Print($"failed");
                }
            }
        }

        public void ExportTourAsPdf(TourDto tour, List<TourLogDto> logs)
        {
            Debug.Print($"Export tour as PDF");

            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = tour.Name,
                Filter = "PDF Document (*.pdf)|*.pdf",
                InitialDirectory = Config["PersistenceFolder"]
            };

            if (dialog.ShowDialog() == true)
            {
                Debug.Print($"Save as {dialog.FileName}");

                if (TourController.ExportTourAsPdf(dialog.FileName, tour, logs))
                {
                    Debug.Print($"succeeded");
                }
                else
                {
                    Debug.Print($"failed");
                }
            }
        }

        public void ExportSummaryAsPdf()
        {
            Debug.Print($"Summary export as PDF");

            SaveFileDialog dialog = new SaveFileDialog()
            {
                FileName = "summary",
                Filter = "PDF Document (*.pdf)|*.pdf",
                InitialDirectory = Config["PersistenceFolder"]
            };

            if (dialog.ShowDialog() == true)
            {
                Debug.Print($"Save as {dialog.FileName}");

                if (TourController.ExportSummaryAsPdf(dialog.FileName))
                {
                    Debug.Print($"succeeded");
                }
                else
                {
                    Debug.Print($"failed");
                }
            }
        }
    }
}
