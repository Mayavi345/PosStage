using Stage.Presentation.Common;
using Stage.Presentation.MVVM.MainPage.ViewModel;
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
using PosStage.MVVM.Models;
using PosStage.MVVM.ViewModel;
using State.CommonWPF;

namespace PosStage.MVVM.View
{
    /// <summary>
    /// NavBarUserControl.xaml 的互動邏輯
    /// </summary>
    public partial class NavBarUserControl : UserControl
    {

        public NavBarUserControl()
        {
            InitializeComponent();
        }

        //public string UserName
        //{
        //    get { return (string)GetValue(NameProperty); }
        //    set { SetValue(NameProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty NameProperty =
        //    DependencyProperty.Register("UserName", typeof(string), typeof(NavBarUserControl), new PropertyMetadata());

        //public string Gender
        //{
        //    get { return (string)GetValue(GenderProperty); }
        //    set { SetValue(GenderProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for Gender.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty GenderProperty =
        //    DependencyProperty.Register("Gender", typeof(string), typeof(NavBarUserControl), new PropertyMetadata());


        //TODO 重購成MVVM
        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            PageManager.Instance.SwitchToPage(EViewPage.MainPageView);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            PageManager.Instance.SwitchToPage(EViewPage.BusinessSummaryView);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            CommonViewSystem.Instance.ReportMainWindow.Show();
            CommonViewSystem.Instance.ReportMainWindow.Activate();

        }

        private void Button_Click_MemberManager(object sender, RoutedEventArgs e)
        {
            PageManager.Instance.SwitchToPage(EViewPage.MemberManagerView);
        }

        private void Button_LoginOut(object sender, RoutedEventArgs e)
        {
            PageManager.Instance.SwitchToPage(EViewPage.LoginView);

        }
    }
}
