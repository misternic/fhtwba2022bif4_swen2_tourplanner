using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TourPlanner.Common.DTO;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class ToursViewModel : BaseViewModel
    {

        private TourDto _selectedTour;
        public TourDto SelectedTour
        {
            get => _selectedTour;
            set
            {
                _selectedTour = value;
                SelectedCommand.Execute(value);
                OnPropertyChanged(nameof(SelectedTour));
            }
        }

        public bool IsSelected
        {
            get => _selectedTour != null;
        }

        public ObservableCollection<TourDto> Tours { get; set; } = new ObservableCollection<TourDto>();

        public ICommand SelectedCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }


        public event EventHandler<TourDto> SelectedEvent;
        public event EventHandler AddEvent;
        public event EventHandler<TourDto> DeleteEvent;

        public ToursViewModel()
        {
            SelectedCommand = new RelayCommand((_) =>
            {
                if (SelectedTour != null)
                {
                    this.SelectedEvent?.Invoke(this, SelectedTour);
                }
            });

            AddCommand = new RelayCommand((_) =>
            {
                this.AddEvent?.Invoke(this, EventArgs.Empty);
            });

            RemoveCommand = new RelayCommand((_) =>
            {
                if (SelectedTour != null) 
                    this.DeleteEvent?.Invoke(this, SelectedTour);
            });
        }

        public void LoadTours(IEnumerable<TourDto> tours)
        {
            Tours.Clear();

            foreach (TourDto item in tours)
            {
                Tours.Add(item);
            }
        }

        public void SelectTour(Guid id)
        {
            SelectedTour = Tours.Where(t => t.Id == id).Single();
        }
        public void SelectTourWithoutEvent(Guid id)
        {
            _selectedTour = Tours.Where(t => t.Id == id).Single();
            OnPropertyChanged(nameof(SelectedTour));
        }
    }
}
