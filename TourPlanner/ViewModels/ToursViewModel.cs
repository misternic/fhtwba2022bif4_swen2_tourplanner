using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.Common;
using TourPlanner.Common.DTO;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class ToursViewModel : BaseViewModel
    {

        private TourDto _selectedTourDto;
        public TourDto SelectedTourDto
        {
            get => _selectedTourDto;
            set
            {
                _selectedTourDto = value;
                SelectedCommand.Execute(value);
                OnPropertyChanged(nameof(SelectedTourDto));
            }
        }

        public ObservableCollection<TourDto> Tours { get; set; } = new ObservableCollection<TourDto>();

        public ICommand SelectedCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand RemoveCommand { get; }


        public event EventHandler<TourDto> SelectedEvent;
        public event EventHandler AddEvent;
        public event EventHandler<TourDto> RemoveEvent;

        public ToursViewModel()
        {
            SelectedCommand = new RelayCommand((_) =>
            {
                if (SelectedTourDto != null)
                {
                    this.SelectedEvent?.Invoke(this, SelectedTourDto);
                }
            });

            AddCommand = new RelayCommand((_) =>
            {
                this.AddEvent?.Invoke(this, EventArgs.Empty);
            });

            RemoveCommand = new RelayCommand((_) =>
            {
                if (SelectedTourDto != null) 
                    this.RemoveEvent?.Invoke(this, SelectedTourDto);
            });
        }
    }
}
