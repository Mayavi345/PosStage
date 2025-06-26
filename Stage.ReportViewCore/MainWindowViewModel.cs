using GalaSoft.MvvmLight;
using Stage.BLL.BLL.Service;
using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Reporting.WinForms;
using PosStage.MVVM.Models;
using System.Data;
using System.Windows;
using System.Data.SqlClient;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Stage.ReportViewCore.Calendar;
using UIComponent.Controls;
using Stage.ReportViewCore.Report;
using System.Windows.Controls;

namespace Stage.ReportViewCore
{
    public class MainWindowViewModel : ObservableObject
    {
        private UserControl _myUserControlInstance;
        public UserControl MyUserControlInstance
        {
            get { return _myUserControlInstance; }
            set
            {
                _myUserControlInstance = value;
                RaisePropertyChanged(nameof(MyUserControlInstance));
            }
        }
        public MainWindowViewModel()
        {
            MyUserControlInstance = new ReportView();

        }

    }
}
