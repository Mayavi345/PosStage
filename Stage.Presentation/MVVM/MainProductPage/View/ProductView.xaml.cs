using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// ProductView.xaml 的互動邏輯
    /// </summary>
    public partial class ProductView : UserControl
    {

        public ProductView()
        {
            InitializeComponent();

            ProductListbox.Items.Clear();

        }


    }
}
