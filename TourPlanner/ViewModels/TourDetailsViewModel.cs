using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TourPlanner.Common;
using TourPlanner.DAL.MapQuest;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourDetailsViewModel : BaseViewModel
    {
        private Tour _tour;
        public Tour Tour
        {
            get => _tour;
            set
            {
                _tour = value;
                OnPropertyChanged(nameof(Tour));
                OnPropertyChanged(nameof(TourIsSelected));
                OnPropertyChanged(nameof(ImagePath));
                OnPropertyChanged(nameof(SelectedTransportType));
            }
        }
        public bool TourIsSelected
        {
            get
            {
                return Tour != null;
            }
        }

        public TransportType SelectedTransportType
        {
            get
            {
                if (Tour == null) return TransportType.Bicycle;
                return _tour.TransportType;
            }
            set
            {
                _tour.TransportType = value;
            }
        }

        public IEnumerable<TransportType> TransportTypesList
        {
            get => Enum.GetValues(typeof(TransportType)).Cast<TransportType>();
        }

        private IConfigurationRoot config = AppSettings.GetInstance().Configuration;
  
        public String ImagePath
        {
            get
            {
                if (_tour == null) return "../images/tour-detail_default.png";

                var path = Path.Combine(config["PersistenceFolder"], $"{_tour.Id}.jpg");

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
