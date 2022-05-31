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

            this.menuVM.AddEvent += (_, e) => this.AddTour();
            this.menuVM.SaveEvent += (_, e) => this.SaveTour(this.toursVM.SelectedTourDto);
            this.menuVM.DeleteEvent += (_, e) => this.RemoveTour(this.toursVM.SelectedTourDto);
            this.menuVM.ExportAsPdfEvent += (_, tour) => this.ExportAsPdf(this.toursVM.SelectedTourDto);

            this.searchVM.SearchEvent += (_, filter) => this.LoadTours(filter);
            this.toursVM.AddEvent += (_, e) => this.AddTour();
            this.tourDetailsVM.SaveEvent += (_, tour) => this.SaveTour(tour);
            this.toursVM.RemoveEvent += (_, tour) => this.RemoveTour(tour);
            this.toursVM.SelectedEvent += (_, tour) => this.TourSelected(tour);
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
            //Debug.Print(this.tourDetailsVM.ImagePath);
            this.LoadTourDetails(tourDto);
            this.LoadTourLogs(tourDto);
        }

        public void LoadTourDetails(TourDto tourDto)
        {
            tourDetailsVM.TourDto = tourDto;
        }

        public void LoadTourLogs(TourDto tourDto)
        {
            tourLogsVM.MyCollection.Clear();

            foreach (TourLogDto item in TourLogController.GetLogsOfTour(tourDto.Id))
            {
                this.tourLogsVM.MyCollection.Add(item);
            }
        }

        public void AddTour()
        {
            Guid newId = Guid.NewGuid();

            TourDto t = new TourDto();
            t.Id = newId;
            t.Name = "New Tour";
            t.Description = "";
            t.From = "";
            t.To = "";
            TourController.AddItem(t);

            this.ClearFilter();
            this.LoadTours();
            this.toursVM.SelectedTourDto = this.toursVM.Tours.Where(t => t.Id == newId).Single();
        }

        public void RemoveTour(TourDto tourDto)
        {
            MessageBoxResult result = MessageBox.Show("Click yes if you want to delete the tour.", "TourPlanner", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                TourController.RemoveItem(tourDto);
                this.LoadTours(this.searchVM.SearchText);
            }
        }

        public async void SaveTour(TourDto tourDto)
        {
            var metaData = await MapQuestService.GetRouteMetaData(tourDto.From, tourDto.To, tourDto.TransportType);

            if (metaData != null)
            {
                tourDto.Distance = metaData?.Distance ?? Double.NegativeInfinity;
                tourDto.EstimatedTime = metaData?.FormattedTime ?? TimeSpan.Zero;

                this.TourSelected(tourDto);
                TourController.UpdateItem(tourDto);
                tourDetailsVM.LoadRouteImageAsync();
            }
        }

        public void ExportAsPdf(TourDto tourDto)
        {
            Debug.Print($"Export tour as PDF");

            SaveFileDialog dia = new SaveFileDialog();
            dia.FileName = tourDto.Name;
            dia.Filter = "PDF Document (*.pdf)|*.pdf";
            dia.InitialDirectory = Config["PersistenceFolder"];

            if (dia.ShowDialog() == true)
            {
                Debug.Print($"Save as {dia.FileName}");

                var report = new TourReport(tourDetailsVM.TourDto, tourLogsVM.TourLog.ToList());
                var success = report.ExportToPdf(dia.FileName);

                if (success) Debug.Print("Export succeeded");
                else Debug.Print("Export failed");
            }
        }
    }
}
