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

namespace UIComponent.View
{
    /// <summary>
    /// PasswordUserControl.xaml 的互動邏輯
    /// </summary>
    public partial class PasswordUserControl : UserControl
    {

        public PasswordUserControl()
        {
            InitializeComponent();
            IsPreviewText = false;
        }

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(PasswordUserControl), new PropertyMetadata(null));


        /// <summary>
        /// 提示字
        /// </summary>
        public string Hint
        {
            get { return (string)GetValue(HintProperty); }
            set { SetValue(HintProperty, value); }
        }
        public static readonly DependencyProperty HintProperty =
            DependencyProperty.Register("Hint", typeof(string), typeof(PasswordUserControl), new PropertyMetadata(null));

        public bool IsPreviewText
        {
            get { return (bool)GetValue(PreviewTextProperty); }
            set { SetValue(PreviewTextProperty, value); }
        }

        public static readonly DependencyProperty PreviewTextProperty =
            DependencyProperty.Register("PreviewText", typeof(bool), typeof(PasswordUserControl), new PropertyMetadata());


        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            //Password = passwordBox.Password;
            PasswordBox pwd = sender as PasswordBox;
            if (pwd != null)
            {
                if (!string.IsNullOrEmpty(pwd.Password))
                {
                    pwd.Background = Brushes.White;
                }
                else
                {
                    pwd.Background = (Brush)FindResource("HelpBrush");
                }
            }
        }

        private void passwordBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (IsPreviewText)
                e.Handled = true;
        }

        private void PasswordBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (IsPreviewText)
                e.Handled = true;
        }
    }
}
