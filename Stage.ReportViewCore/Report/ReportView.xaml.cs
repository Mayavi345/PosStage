using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Stage.ReportViewCore.Report
{
    /// <summary>
    /// ReportView.xaml 的互動邏輯
    /// </summary>
    public partial class ReportView : UserControl
    {
        ReportWindowViewModel ReportWindowViewModel { get; set; }
        public ReportView()
        {
            InitializeComponent();
            //TODO 統一初始化時機
            MainSystemService.Instance.InitSystem(EDataDb.MsSqlServer, EServiceType.SQL);
            ReportWindowViewModel = new ReportWindowViewModel();
            ReportWindowViewModel.Init(myReportViewr, MemberConsumptionReport);
            DataContext = ReportWindowViewModel;

        }
    }
}
