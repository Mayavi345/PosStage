using AdonisUI;
using AdonisUI.Controls;
using System.Windows;
using PosStage.MVVM.ViewModel;

namespace PosStage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow 
    {
        //TODO 把MainWindows整理乾淨，把程式搬運到MainViewModel
        public static MainWindow Instance { get; set; }

        public const string ConfigFileName = "Setting";

        public MainWindow()
        {
            InitializeComponent();
            ResourceLocator.SetColorScheme(Application.Current.Resources, false ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme);
            Instance = this;
            DataContext = this;
        }
    }
}
