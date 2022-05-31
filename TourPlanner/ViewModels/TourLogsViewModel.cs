using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Common;
using TourPlanner.Common.DTO;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourLogsViewModel : BaseViewModel
    {
        private IEnumerable<TourLogDto> _tourLog;
        public IEnumerable<TourLogDto> TourLog
        {
            get => _tourLog;
            set
            {
                _tourLog = value;
                MyCollection = new ObservableCollection<TourLogDto>(value);
                OnPropertyChanged(nameof(TourLog));
            }
        }

        public ObservableCollection<TourLogDto> MyCollection { get; set; } = new ObservableCollection<TourLogDto>();


        public static IEnumerable<Difficulty> GetDifficultyEnumTypes => Enum.GetValues(typeof(Difficulty)).Cast<Difficulty>();
    }
}
