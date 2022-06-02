using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TourPlanner.Common.DTO;
using TourPlanner.Common.Logging;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourLogsViewModel : BaseViewModel
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

        public ObservableCollection<TourLogDto> TourLogs { get; set; } = new ObservableCollection<TourLogDto>();

        public TourLogsViewModel()
        {
            TourLogs.CollectionChanged += (sender, e) =>
            {
                logger.Debug(e.Action.ToString());
                /*if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    // do stuff
                }*/
            };
        }

        public void LoadTourLogs(IEnumerable<TourLogDto> logs)
        {
            this.TourLogs = new ObservableCollection<TourLogDto>(logs);



            /*this.TourLogs.Clear();

            foreach (TourLogDto item in logs)
            {
                this.TourLogs.Add(item);
            }*/
        }
    }
}
