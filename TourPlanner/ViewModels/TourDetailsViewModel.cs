using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
                OnPropertyChanged(nameof(RouteImage));
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
        public ICommand DeleteCommand { get; }
        public ICommand ExportAsPdfCommand { get; }

        public event EventHandler<TourDto> SaveEvent;
        public event EventHandler<TourDto> DeleteEvent;
        public event EventHandler<TourDto> ExportAsPdfEvent;

        public TourDetailsViewModel()
        {
            SaveCommand = new RelayCommand((_) =>
            {
                this.SaveEvent?.Invoke(this, _tour);
            });
            DeleteCommand = new RelayCommand((_) =>
            {
                this.DeleteEvent?.Invoke(this, _tour);
            });
            ExportAsPdfCommand = new RelayCommand((_) =>
            {
                this.ExportAsPdfEvent?.Invoke(this, _tour);
            });
        }

        public BitmapImage RouteImage
        {
            get
            {
                var path = Path.Combine(Config["PersistenceFolder"], $"{_tour?.Id}.jpg");

                if (_tour == null || !File.Exists(path.ToString()))
                    return new BitmapImage(new Uri(@"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/" + "Images/tour-detail_default.png", UriKind.Absolute));

                return LoadBitmapImage(path);
            }
        }

        public static BitmapImage LoadBitmapImage(string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze(); 
                return bitmapImage;
            }
        }
    }
}
