using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BL;
using TourPlanner.Common;
using TourPlanner.Common.DTO;
using TourPlanner.Common.Logging;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourDetailsViewModel : BaseViewModel
    {
        private static ILoggerWrapper logger = LoggerFactory.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Visibility _visibility = Visibility.Hidden;
        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
                OnPropertyChanged(nameof(Visibility));
            }
        }



        private TourDto _tour;
        public TourDto Tour
        {
            get => _tour;
            set
            {
                _tour = value;
                OnPropertyChanged(nameof(Tour));
                OnPropertyChanged(nameof(ImagePath));
                OnPropertyChanged(nameof(SelectedTransportType));
            }
        }

        public TransportType SelectedTransportType
        {
            get => _tour?.TransportType ?? TransportType.Bicycle;
            set
            {
                _tour.TransportType = value;
            }
        }

        public static IEnumerable<TransportType> GetTransportTypeEnumTypes => Enum.GetValues(typeof(TransportType)).Cast<TransportType>();

        public ICommand SaveCommand { get; }
        public event EventHandler<TourDto> SaveEvent;

        public TourDetailsViewModel()
        {
            SaveCommand = new RelayCommand((_) =>
            {
                this.SaveEvent?.Invoke(this, _tour);
            });
        }

        public String ImagePath
        {
            get
            {
                if (_tour == null) return "../images/tour-detail_default.png";

                var path = Path.Combine(Config["PersistenceFolder"], $"{_tour.Id}.jpg");

                if (!File.Exists(path)) return "../images/tour-detail_default.png";

                return path;
            }
        }
    }
}
