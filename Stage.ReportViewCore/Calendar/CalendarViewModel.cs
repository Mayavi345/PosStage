using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Stage.ReportViewCore.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using UIComponent.Controls;
using UIComponent.View;

namespace Stage.ReportViewCore.Calendar
{
    public class CalendarViewModel : ObservableObject
    {
        private DateRange _viewModelDate;

        public CalendarViewModel()
        {
            ApplyCommand = new RelayCommand(ApplyAcion);
            ClearCommand = new RelayCommand(ClearAction);
        }

        public DateRange ViewModelDateList
        {
            get { return _viewModelDate; }
            set
            {
                if (_viewModelDate != value)
                {
                    _viewModelDate = value;
                    RaisePropertyChanged(nameof(ViewModelDateList));
                }
            }
        }

        public ICommand ApplyCommand { get; set; }
        public ICommand ClearCommand { get; set; }


        private void ApplyAcion()
        {
            Messenger.Default.Send(ViewModelDateList, "DateRangeChange");
        }

        private void ClearAction()
        {

        }
    }
}
