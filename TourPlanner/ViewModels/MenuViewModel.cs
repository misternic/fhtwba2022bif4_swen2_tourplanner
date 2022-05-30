using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand ExportAsPdfCommand { get; }

        public event EventHandler<Object> ExportAsPdfEvent;

        public MenuViewModel()
        {
            ExportAsPdfCommand = new RelayCommand((_) =>
            {
                this.ExportAsPdfEvent?.Invoke(this, new Object());
            });
        }
    }
}
