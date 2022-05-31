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
        public ICommand ExportCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand SummaryExportAsPdfCommand { get; }
        public ICommand ExitCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ExportAsPdfCommand { get; }

        public event EventHandler AddEvent;
        public event EventHandler ExportEvent;
        public event EventHandler ImportEvent;
        public event EventHandler SummaryExportAsPdfEvent;
        public event EventHandler SaveEvent;
        public event EventHandler DeleteEvent;
        public event EventHandler ExportAsPdfEvent;

        public MenuViewModel()
        {
            AddCommand = new RelayCommand((_) =>
            {
                this.AddEvent?.Invoke(this, EventArgs.Empty);
            });

            ExportCommand = new RelayCommand((_) =>
            {
                this.ExportEvent?.Invoke(this, EventArgs.Empty);
            });

            ImportCommand = new RelayCommand((_) =>
            {
                this.ImportEvent?.Invoke(this, EventArgs.Empty);
            });

            SummaryExportAsPdfCommand = new RelayCommand((_) =>
            {
                this.SummaryExportAsPdfEvent?.Invoke(this, EventArgs.Empty);
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
                this.ExportAsPdfEvent?.Invoke(this, EventArgs.Empty);
            });
        }
    }
}
