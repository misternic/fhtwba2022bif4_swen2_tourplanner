using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Common;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourLogsViewModel : BaseViewModel
    {
        private IEnumerable<TourLog> _tourLog;
        public IEnumerable<TourLog> TourLog
        {
            get => _tourLog;
            set
            {
                _tourLog = value;
                MyCollection = new ObservableCollection<TourLog>(value);
                OnPropertyChanged(nameof(TourLog));
            }
        }

        public ObservableCollection<TourLog> MyCollection { get; set; } = new ObservableCollection<TourLog>();


        public static IEnumerable<Difficulty> GetDifficultyEnumTypes => Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>();
    }
}
