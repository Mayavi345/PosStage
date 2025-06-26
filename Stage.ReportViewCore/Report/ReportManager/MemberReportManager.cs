using Microsoft.Reporting.WinForms;
using Stage.BLL.BLL.Service;
using Stage.BLL.BLL;
using System;
using System.Data;
using UIComponent.View;
using Stage.ReportViewCore.Service;

namespace Stage.ReportViewCore
{
    public class MemberReportManager : ReportViewManagerBase
    {
        private DateRange _dateRange;
        IReportService _reportSerivce;
        public MemberReportManager(ReportViewer myReportViewr) : base(myReportViewr)
        {
            _reportSerivce = MainSystemService.Instance.ReportService;
        }
        public void SetDateRange(DateRange dateRange)
        {
            _dateRange = dateRange;
            RefreshReportView();

        }
        public override void RefreshReportView()
        {
            base.RefreshReportView();
            DataTable dt;
            if (_dateRange != null)
            {
                dt = _reportSerivce.GetOrderDetailReportData(_dateRange.StartDate, _dateRange.EndDate);
            }
            else
            {
                dt = _reportSerivce.GetOrderDetailReportData(DateTime.Now, DateTime.Now);
            }
            ReportHelper.UpdateRepoertViewData(myReportView, "DataSet1", dt, "Stage.ReportViewCore.Report1.rdlc");
        }
    }

}
