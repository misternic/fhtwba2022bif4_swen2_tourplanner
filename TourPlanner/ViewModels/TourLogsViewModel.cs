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

        private TourLogDto _selectedTourLog;
        public TourLogDto SelectedTourLog
        {
            get => _selectedTourLog;
            set
            {
                _selectedTourLog = value;
                OnPropertyChanged(nameof(SelectedTourLog));
            }
        }

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
                if (_selectedTourLog == null) return;
                this.DeleteEvent?.Invoke(this, _selectedTourLog);
            }, (_) =>
            {
                return _selectedTourLog != null;
            });
            EditComand = new RelayCommand((_) =>
            {
                this.EditEvent?.Invoke(this, _selectedTourLog);
            }, (_) =>
            {
                return _selectedTourLog != null;
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
