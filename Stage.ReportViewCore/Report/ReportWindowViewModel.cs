using GalaSoft.MvvmLight;
using Microsoft.Reporting.WinForms;
using PosStage.MVVM.Models;
using Stage.BLL.BLL.Service;
using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using System.Windows;
using UIComponent.View;
using Microsoft.Data.SqlClient;
using Stage.ReportViewCore.Service;
using Stage.ReportViewCore.Calendar;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace Stage.ReportViewCore
{
    public class ReportWindowViewModel : ObservableObject
    {
        public ICommand RefreshCommand { get; }
        public ICommand ExitCommand { get; }

        private ReportViewManagerBase _reportViewMember;



        private ReportViewManagerBase _consumptionReportManager;

        public ReportWindowViewModel()
        {
            RefreshCommand = new RelayCommand(RefreshReportData);
            CalendarViewModel = new CalendarViewModel();
            Messenger.Default.Register<DateRange>(this, "DateRangeChange", DateRangeChange);

        }



        private CalendarViewModel _calendarViewModel;
        public CalendarViewModel CalendarViewModel
        {
            get { return _calendarViewModel; }
            set
            {
                if (_calendarViewModel != value)
                {
                    _calendarViewModel = value;
                    RaisePropertyChanged(nameof(CalendarViewModel));
                }
            }
        }
        public void Init(ReportViewer myReportViewr, ReportViewer MemberConsumptionReport)
        {
            _reportViewMember = new MemberReportManager(myReportViewr);
            _reportViewMember.RefreshReportView();

            _consumptionReportManager = new ConsumptionReportManager(MemberConsumptionReport);
            _consumptionReportManager.RefreshReportView();
        }

        private void RefreshReportData()
        {
            _reportViewMember.RefreshReportView();
            _consumptionReportManager.RefreshReportView();

        }
        private void DateRangeChange(DateRange range)
        {
            (_reportViewMember as MemberReportManager).SetDateRange(range);
        }
        #region 沒用到的程式碼
        private DataTable GetData2()
        {
            var memberRepository = MainSystemService.Instance.DataFactory.MemberRepository;
            List<MemberConsumptionReportModel> memberConsumptionReportModelsList = memberRepository.GetMemberConsumptionReportModel();

            DataTable dataTable = new DataTable("MemberCunsumptionDataSet");


            dataTable.Columns.Add("MemberName", typeof(string));
            dataTable.Columns.Add("MemberGender", typeof(string));
            dataTable.Columns.Add("ConsumptionName", typeof(int));
            dataTable.Columns.Add("OrderTotalPrice", typeof(double));
            dataTable.Columns.Add("OrderDate", typeof(DateTime));

            foreach (var item in memberConsumptionReportModelsList)
            {
                dataTable.Rows.Add(item.MemberName, item.MemberGender, item.ConsumptionName, item.OrderTotalPrice, item.OrderDate);

                //DataRow row = dataTable.NewRow();
                //row[nameof(item.MemberName)] = item.MemberName;
                //row[nameof(item.MemberGender)] = item.MemberGender;
                //row[nameof(item.ConsumptionName)] = item.ConsumptionName;
                //row[nameof(item.OrderTotalPrice)] = item.OrderTotalPrice;                row[nameof(item.OrderTotalPrice)] = item.OrderTotalPrice;
                //row[nameof(item.OrderDate)] = item.OrderDate;
                //dataTable.Rows.Add(row);
            }

            //DataSet dataSet = new DataSet();
            //dataSet.Tables.Add(dataTable);

            return dataTable;

        }
        #endregion
    }
}
