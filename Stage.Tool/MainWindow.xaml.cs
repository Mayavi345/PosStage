using AdonisUI;
using AdonisUI.Controls;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Stage.BLL.BLL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Utilities;
using Utilities.Nlog;
using Utilities.Observer;

namespace Stage.Tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : AdonisWindow
    {
        MainWindow currentWindow;
        public MainWindow()
        {
            InitializeComponent();
            currentWindow = this;
            ResourceLocator.SetColorScheme(Application.Current.Resources, false ? ResourceLocator.LightColorScheme : ResourceLocator.DarkColorScheme);
            //dbDataFactory = new DbDataFactory();
            //UIMessageBox = new MessageSubject();
            //InitDataSystem();

            MainConfigService.Instance.InitConfig();

        }
        public static string CurrentProjectPath()
        {
            try
            {
                // 取得當前執行的Assembly
                Assembly currentAssembly = Assembly.GetAssembly(typeof(MainWindow));

                Assembly mainProjectAssembly;
                // 取得主專案的 Assembly，假設你知道它的名稱
                 mainProjectAssembly = AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(a => a.FullName.StartsWith("Stage.Presentation"));
                if (mainProjectAssembly == null)
                {
                    mainProjectAssembly = AppDomain.CurrentDomain.GetAssemblies()
                       .FirstOrDefault(a => a.FullName.StartsWith("Stage.Backstage"));
                }
                // 取得該Assembly的路徑
                string assemblyPath = mainProjectAssembly.Location;
                // 在路徑中找到config檔案
                string configFilePath = Path.Combine(Path.GetDirectoryName(assemblyPath), "Setting");
                return configFilePath;
            }
            catch (Exception e)
            {
                //開啟本地專案的Setting
                MainConfigService.Instance.InitConfig();
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                return MainConfigService.Instance.ConnectionString;
            }
        }

        public MessageSubject UIMessageBox;
        DbDataBase dbDataFactory;
        public void InitDataSystem()
        {
            MainConfigService.Instance.InitConfig();
            dbDataFactory = new EFMsSQLServerData(MainConfigService.Instance.ConnectionString);
            MainSystemService.Instance.InitSystem(EDataDb.MsSqlServer, EServiceType.SQL);
            dbDataFactory.InitRepository();
        }

        private void button_Save(object sender, RoutedEventArgs e)
        {
            currentWindow.Close();
        }

        private void button_Cancel(object sender, RoutedEventArgs e)
        {
            currentWindow.Close();
        }

        private void button_Choose(object sender, RoutedEventArgs e)
        {


        }
    }
}
