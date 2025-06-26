using GalaSoft.MvvmLight;
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
    /// LabelTextUserControl1.xaml 的互動邏輯
    /// </summary>
    public partial class LabelTextUserControl1 : UserControl
    {
        public LabelTextUserControl1()
        {
            InitializeComponent();
        }
        public string TextboxText
        {
            get { return (string)GetValue(TextboxTextProperty); }
            set { SetValue(TextboxTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextboxText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextboxTextProperty =
            DependencyProperty.Register("TextboxText", typeof(string), typeof(LabelTextUserControl1), new PropertyMetadata(String.Empty, PasswordPropertyChanged));

        private static void PasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LabelTextUserControl1 item)
            {
                item.UpdateText();
            }
        }

        private void UpdateText()
        {
            textBox.Text = TextboxText;
        }

        private int _currentEditCombo;

        public bool IsEnable
        {
            get { return (bool)GetValue(IsEnableProperty); }
            set { SetValue(IsEnableProperty, value); }
        }

        public static readonly DependencyProperty IsEnableProperty =
            DependencyProperty.Register("IsEnable", typeof(bool), typeof(LabelTextUserControl1), new PropertyMetadata());


    }
}
