using System;
using System.Windows.Input;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class SearchViewModel : BaseViewModel
    {
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private string _searchText = "";

        public ICommand SearchCommand { get; }

        public event EventHandler<string> SearchEvent;

        public SearchViewModel()
        {
            SearchCommand = new RelayCommand((_) =>
            {
                this.SearchEvent?.Invoke(this, this.SearchText);
            });
        }
    }
}
