﻿using Microsoft.Win32;
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

            this.searchVM.SearchEvent += (_, filter) => this.LoadTours(filter);
            this.toursVM.AddEvent += (_, e) => this.AddTour();
            this.toursVM.RemoveEvent += (_, tour) => this.RemoveTour(tour);
            this.toursVM.SelectedEvent += (_, tour) => this.TourSelected(tour);
            this.menuVM.ExportAsPdfEvent += (_, tour) => this.ExportAsPdf();
        }
        public void LoadTours(string filter = null)
        {
            this.toursVM.Tours.Clear();

            foreach (Tour item in TourController.GetItems(filter))
            {
                this.toursVM.Tours.Add(item);
            }
        }

        public void ClearFilter()
        {
            this.searchVM.SearchText = String.Empty;
        }

        public void TourSelected(Tour tour)
        {
            Debug.Print($"Tour selected: {tour.Id} {tour.Name}");
            Debug.Print(tour.ToJson());
            //Debug.Print(this.tourDetailsVM.ImagePath);
            this.LoadTourDetails(tour);
            this.LoadTourLogs(tour);
            tourDetailsVM.LoadRouteImageAsync();
        }

        public void LoadTourDetails(Tour tour)
        {
            tourDetailsVM.Tour = tour;
        }

        public void LoadTourLogs(Tour tour)
        {
            tourLogsVM.TourLog = TourLogController.GetLogsOfTour(tour.Id);
        }

        public void AddTour()
        {
            Guid newId = Guid.NewGuid();

            Tour t = new Tour();
            t.Id = newId;
            t.Name = "New Tour";
            t.Description = "";
            t.From = "";
            t.To = "";
            TourController.AddItem(t);

            this.ClearFilter();
            this.LoadTours();
            this.toursVM.SelectedTour = this.toursVM.Tours.Where(t => t.Id == newId).Single();
        }

        public void RemoveTour(Tour tour)
        {
            TourController.RemoveItem(tour);
            this.LoadTours(this.searchVM.SearchText);
        }

        public void SaveTour(Tour tour)
        {
            // TODO
        }

        public void ExportAsPdf()
        {
            Debug.Print($"Export tour as PDF");

            SaveFileDialog dia = new SaveFileDialog();
            dia.FileName = tourDetailsVM.Tour.Name;
            dia.Filter = "PDF Document (*.pdf)|*.pdf";
            dia.InitialDirectory = Config["PersistenceFolder"];

            if (dia.ShowDialog() == true)
            {
                Debug.Print($"Save as {dia.FileName}");

                if (tourDetailsVM.TourIsSelected)
                {
                    var report = new TourReport(tourDetailsVM.Tour, tourLogsVM.TourLog.ToList());
                    var success = report.ExportToPdf(dia.FileName);

                    if (success) Debug.Print("Export succeeded");
                    else Debug.Print("Export failed");
                }
            }
        }
    }
}
