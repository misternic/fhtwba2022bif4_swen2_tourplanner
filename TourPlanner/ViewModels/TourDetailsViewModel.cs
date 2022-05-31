using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TourPlanner.Common;
using TourPlanner.Common.DTO;
using TourPlanner.DAL.MapQuest;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourDetailsViewModel : BaseViewModel
    {
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

        public async Task LoadRouteImageAsync()
        {
            if (await MapQuestService.GetRouteImage(_tour.Id.ToString(), _tour.From, _tour.To))
            {
                Debug.Print("RouteImage true");
            } else 
            {
                Debug.Print("RouteImage false");
            }
            OnPropertyChanged(nameof(ImagePath));
        }
    }
}
