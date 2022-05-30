using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.ViewModels.Abstract;

namespace TourPlanner.ViewModels
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ExportAsPdfCommand { get; }

        public event EventHandler AddEvent;
        public event EventHandler SaveEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler<Object> ExportAsPdfEvent;

        public MenuViewModel()
        {
            AddCommand = new RelayCommand((_) =>
            {
                this.AddEvent?.Invoke(this, EventArgs.Empty);
            });

            ExitCommand = new RelayCommand((_) =>
            {
                Application.Current.Shutdown();
            });

            SaveCommand = new RelayCommand((_) =>
            {
                this.SaveEvent?.Invoke(this, EventArgs.Empty);
            });

            DeleteCommand = new RelayCommand((_) =>
            {
                this.DeleteEvent?.Invoke(this, EventArgs.Empty);
            });

            ExportAsPdfCommand = new RelayCommand((_) =>
            {
                this.ExportAsPdfEvent?.Invoke(this, new Object());
            });
        }
    }
}
