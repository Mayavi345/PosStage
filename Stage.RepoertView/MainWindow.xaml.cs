using Microsoft.Reporting.WebForms;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using DataSet = System.Data.DataSet;

namespace Stage.RepoertView
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            RefreshReportData();
        }


        private void RefreshReportData()
        {
            myReportViewr.Reset();
            DataTable dt = GetData();
            ReportDataSource reportDataSource = new ReportDataSource("DataSet2", dt);
            myReportViewr.LocalReport.DataSources.Add(reportDataSource);
            myReportViewr.LocalReport.ReportEmbeddedResource = "Stage.RepoertView.Report1.rdlc";
            myReportViewr.RefreshReport();
        }

        private DataTable GetData()
        {
            string connectString = Utilities.MainConfigService.Instance.ConnectionString;

            string SQLString = "GetViewProductData";
            SqlConnection sqlconnection = new SqlConnection(connectString);
            DataSet dataset = new DataSet();
            SqlCommand sqlcommand = new SqlCommand(SQLString);

            sqlcommand.Connection = sqlconnection;
            sqlcommand.Parameters.Clear();

            using (sqlcommand.Connection)
            {
                sqlcommand.Connection.Open();
                SqlDataAdapter sqldataadapter = new SqlDataAdapter(sqlcommand);
                sqldataadapter.Fill(dataset);

            }
            return dataset.Tables[0];
        }

        private void Button_Click_Refresh(object sender, RoutedEventArgs e)
        {
            RefreshReportData();

        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}
