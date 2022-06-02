using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TourPlanner.Common.DTO;
using TourPlanner.Common.Logging;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourLogsViewModel : BaseViewModel
    {
        private static ILoggerWrapper logger = LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Visibility _visibility = Visibility.Hidden;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }

        public ObservableCollection<TourLogDto> TourLogs { get; set; } = new ObservableCollection<TourLogDto>();

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand EditComand { get; }

        public event EventHandler AddEvent;
        public event EventHandler<TourLogDto> DeleteEvent;
        public event EventHandler<TourLogDto> EditEvent;

        public TourLogsViewModel()
        {
            AddCommand = new RelayCommand((_) =>
            {
                this.AddEvent?.Invoke(this, EventArgs.Empty);
            });
            DeleteCommand = new RelayCommand((_) =>
            {
                this.DeleteEvent?.Invoke(this, new TourLogDto());
            });
            EditComand = new RelayCommand((_) =>
            {
                this.EditEvent?.Invoke(this, new TourLogDto());
            });
        }

        public void LoadTourLogs(IEnumerable<TourLogDto> logs)
        {
            this.TourLogs.Clear();

            foreach (TourLogDto item in logs)
            {
                this.TourLogs.Add(item);
            }
        }
    }
}
