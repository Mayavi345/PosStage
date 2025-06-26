using Microsoft.Reporting.WinForms;
using PosStage.MVVM.Models;
using System.Collections.Generic;
using System.Data;

namespace Stage.ReportViewCore.Service
{
    public class ReportHelper
    {
        public static void UpdateRepoertViewData(ReportViewer reportView, string dataSet, DataTable dt, string reportEmbeddedResource)
        {
            ReportDataSource reportDataSource = new ReportDataSource(dataSet, dt);
            reportView.LocalReport.DataSources.Add(reportDataSource);
            reportView.LocalReport.ReportEmbeddedResource = reportEmbeddedResource;
            reportView.RefreshReport();
        }
        public static void UpdateRepoertViewData<T>(ReportViewer reportView, string dataSet, IEnumerable<T> list, string reportEmbeddedResource)
        {
            ReportDataSource reportDataSource = new ReportDataSource(dataSet, list);
            reportView.LocalReport.DataSources.Add(reportDataSource);
            reportView.LocalReport.ReportEmbeddedResource = reportEmbeddedResource;
            reportView.RefreshReport();
        }
    }
}
