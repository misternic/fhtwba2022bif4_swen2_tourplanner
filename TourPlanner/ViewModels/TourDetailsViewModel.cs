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
        private TourDto _tourDto;
        public TourDto TourDto
        {
            get => _tourDto;
            set
            {
                _tourDto = value;
                OnPropertyChanged(nameof(TourDto));
                OnPropertyChanged(nameof(ImagePath));
                OnPropertyChanged(nameof(SelectedTransportType));
            }
        }

        public TransportType SelectedTransportType
        {
            get
            {
                if (_tourDto == null) return TransportType.Bicycle;
                return _tourDto.TransportType;
            }
            set
            {
                _tourDto.TransportType = value;
            }
        }

        public static IEnumerable<TransportType> GetTransportTypeEnumTypes => Enum.GetValues(typeof(TransportType)).Cast<TransportType>();

        public ICommand SaveCommand { get; }
        public event EventHandler<TourDto> SaveEvent;

        public TourDetailsViewModel()
        {
            SaveCommand = new RelayCommand((_) =>
            {
                this.SaveEvent?.Invoke(this, _tourDto);
            });
        }

        public String ImagePath
        {
            get
            {
                if (_tourDto == null) return "../images/tour-detail_default.png";

                var path = Path.Combine(Config["PersistenceFolder"], $"{_tourDto.Id}.jpg");

                if (!File.Exists(path)) return "../images/tour-detail_default.png";

                return path;
            }
        }

        public async Task LoadRouteImageAsync()
        {
            if (await MapQuestService.GetRouteImage(_tourDto.Id.ToString(), _tourDto.From, _tourDto.To))
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
