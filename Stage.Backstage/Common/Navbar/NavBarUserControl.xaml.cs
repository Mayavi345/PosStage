using System.Windows;
using System.Windows.Controls;

namespace Stage.Backstage
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

        public string UserName
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Name.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(NavBarUserControl), new PropertyMetadata());

        public string Gender
        {
            get { return (string)GetValue(GenderProperty); }
            set { SetValue(GenderProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Gender.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GenderProperty =
            DependencyProperty.Register("Gender", typeof(string), typeof(NavBarUserControl), new PropertyMetadata());
  
    }
}
