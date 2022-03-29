using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Common;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class SidebarViewModel : BaseViewModel
    {

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get => _selectedTour;
            set
            {
                _selectedTour = value;
                SelectedCommand.Execute(value);
                OnPropertyChanged(nameof(SelectedTour));
            }
        }

        public ObservableCollection<Tour> Tours { get; set; } = new ObservableCollection<Tour>();

        public ICommand SelectedCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }


        public event EventHandler<Tour> SelectedEvent;
        public event EventHandler AddEvent;
        public event EventHandler<Tour> RemoveEvent;

        public SidebarViewModel()
        {
            SelectedCommand = new RelayCommand((_) =>
            {
                if (SelectedTour != null)
                {
                    Debug.Print($"Tour selected: {SelectedTour.Id} {SelectedTour.Name}");
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
                    this.RemoveEvent?.Invoke(this, SelectedTour);
            });
        }
    }
}
