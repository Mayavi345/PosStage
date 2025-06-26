using Microsoft.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using Stage.ReportViewCore.Service;
using System;
using System.Data;

namespace Stage.ReportViewCore
{
    public class ReportViewManagerBase
    {
        public ReportViewer myReportView { get; set; }
        public ReportViewManagerBase(ReportViewer myReportView) {
            this.myReportView = myReportView;
        }
        public virtual void RefreshReportView() {
            myReportView.Reset();
        }

    }
}