using Stage.Backstage.Common;
using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using UIComponent;

namespace Stage.Backstage
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private void OnStartup(object s, StartupEventArgs e)
        {
            MainSystemService.Instance.ObserverUIMessageBox.RegisterObserver(new UIMessageObserver());
            MainSystemService.Instance.InitSystem(EDataDb.MsSqlServer, EServiceType.SQL);
            ImageSourceProcess.Instance.Init();
            var mainWindow = new MainWindow();
            mainWindow.DataContext = new MainWindowsViewModel();
            mainWindow.Show();

        }

    }
}
