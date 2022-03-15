using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class SearchBarViewModel : BaseViewModel
    {
        public string SearchText
        {
            get => _searchText;
            set
            {
                Debug.Print(value);
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private string _searchText = "";

        public ICommand SearchCommand { get; }

        public event EventHandler<string> SearchEvent;

        public SearchBarViewModel()
        {
            SearchCommand = new RelayCommand((_) =>
            {
                this.SearchEvent?.Invoke(this, this.SearchText);
            }, (_) =>
            {
                return this.SearchText.Length > 0;
            });
        }
    }
}
