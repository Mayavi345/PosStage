using GalaSoft.MvvmLight.Command;
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
using PosStage.MVVM.ViewModel;

namespace PosStage.MVVM.View
{
    /// <summary>
    /// ProductUserControl.xaml 的互動邏輯
    /// </summary>
    public partial class ProductSlotView : UserControl
    {
        public ProductSlotView()
        {
            InitializeComponent();

        }

   
        #region Old

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {

            //int count = Convert.ToInt32(Count);
            //if (count >= 0)
            //{
            //    Count = (count = count + 1).ToString();
            //    UpdateCount();
            //}
        }



        private void Button_Click_Decress(object sender, RoutedEventArgs e)
        {
            //int count = Convert.ToInt32(Count);
            //if (count >= 0)
            //{
            //    Count = (count = count - 1).ToString();
            //    UpdateCount();
            //}
        }
        private void UpdateCount()
        {
            //int count = Convert.ToInt32(Count);
            //int price = Convert.ToInt32(Price);

            //CountPrice = ((count * price).ToString());
        }
        #endregion
    }
}
