using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Common;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class TourLogsViewModel : BaseViewModel
    {
        private List<TourLog> _tourLog;
        public List<TourLog> TourLog
        {
            get => _tourLog;
            set
            {
                _tourLog = value;
                OnPropertyChanged(nameof(TourLog));
            }
        }
    }
}
