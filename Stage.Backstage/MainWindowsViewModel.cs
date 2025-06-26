using GalaSoft.MvvmLight;
using Stage.BLL;
using Stage.BLL.BLL;
using Stage.Presentation.Common;
using System;
using System.Windows.Controls;
using UIComponent.View;
using Utilities.Nlog;

namespace Stage.Backstage
{
    public class MainWindowsViewModel : ObservableObject
    {
        //public ObservableObject CurrentViewModel { get; set; }
        #region Serivce

        #endregion
        public MainWindowsViewModel()
        {
            try
            {
                //CurrentViewModel = this;
                IPageHelper pageHelper = new BackstagePageHelper();
                PageManager.Instance.Init(SwitchUserControl, pageHelper);
                PageManager.Instance.SwitchToPage(EViewPage.LoginView);

                InitService();
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }

        private void InitService()
        {

        }
        #region ViewPage

        /// <summary>
        /// 基礎顯示UserControl的元件
        /// </summary>
        #region Base
        private UserControl _content;

        public UserControl MyUserControlInstance
        {
            get { return _content; }
            set
            {
                _content = value;
                RaisePropertyChanged("MyUserControlInstance");
            }
        }

        #endregion

        public void SwitchUserControl(UserControl userControl)
        {
            MyUserControlInstance = userControl;
        }




        #endregion
    }
}
