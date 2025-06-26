using Microsoft.Reporting.NETCore;
using System.Data;
using System.Windows;
using Microsoft.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using System;
using Stage.BLL.BLL;
using Stage.DAL;
using System.Windows.Documents;
using PosStage.MVVM.Models;
using System.Collections.Generic;
using Stage.DAL.Repositories.Implement;
using Stage.BLL.BLL.Service;
using Stage.ReportViewCore.Calendar;
using System.ComponentModel;
using UIComponent.Controls;
using Utilities;

namespace Stage.ReportViewCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            MainWindowViewModel mainWIndowViewModel = new MainWindowViewModel();
            this.DataContext = mainWIndowViewModel;
            // 註冊 Closing 事件處理程序
            Closing += MainWindow_Closing;
            //TODO 研究Rerport View的Binding方法
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            // 取消關閉操作
            e.Cancel = true;

            // 隱藏視窗而不是關閉
            this.Hide();
        }
    }
}
