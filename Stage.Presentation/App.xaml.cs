using Stage.BLL.BLL;
using System;
using System.Windows;
using UIComponent;
using NLog;
using NLog.Config;
using NLog.Targets;
using Utilities.Nlog;
using Stage.Presentation.Common;

namespace PosStage
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App()
        {

            SplashScreen s = new SplashScreen("loading.gif");
            s.Show(true);
            s.Close(new TimeSpan(0, 0, 3));


        }
        private void OnStartup(object s, StartupEventArgs e)
        {
            LogManagerSingleton.Instance.PrintLog("==============OnStartup=============", NLog.LogLevel.Info);
            State.CommonWPF.CommonViewSystem.Instance.Init();
            MainSystemService.Instance.ObserverUIMessageBox.RegisterObserver(new UIMessageObserver());
            MainSystemService.Instance.InitSystem(EDataDb.MsSqlServer, EServiceType.SQL);
            ImageSourceProcess.Instance.Init();
            var mainWindow = new MainWindow();
            MainWindowViewModel mainViewModel = new MainWindowViewModel();
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
            PageManager.Instance.SwitchToPage(EViewPage.LoginView);
            MainDataCenter.Instance.LoadData();
            //// 在需要時解析DbContext
            //using (var dbContext = _serviceProvider.GetService<PosDbContext>())
            //{
            //    // 創建窗口並傳遞DbContext
            //    var mainWindow = new MainWindow(dbContext);
            //    mainWindow.Show();
            //}
        }
        private static void CreateLogger()
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget
            {
                FileName = "${basedir}/logs/${shortdate}.log",
                Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss} [${uppercase:${level}}] ${message}",
            };
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, fileTarget);
            LogManager.Configuration = config;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {

        }
    }
}
