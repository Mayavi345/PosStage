using GalaSoft.MvvmLight;
using Stage.BLL;
using Stage.BLL.BLL;
using Stage.Presentation.Common;
using System.Windows.Controls;
using UIComponent.View;
using PosStage.MVVM.View;
using PosStage.MVVM.ViewModel;
using System;
using Utilities.Nlog;
using NLog;
using Stage.BLL.BLL.Service;

namespace PosStage
{
    public class MainWindowViewModel : ObservableObject
    {
        public MainWindowViewModel()
        {
            try
            {
                InitSerivce();
                IPageHelper pageHelper = new PageHelper();
                PageManager.Instance.Init(SwitchUserControl, pageHelper);
            }
            catch (Exception e)
            {

                LogManagerSingleton.Instance.PrintLog(e.Message, LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }

        public void SwitchUserControl(UserControl userControl)
        {
            MyUserControlInstance = userControl;
        }

        #region Service
        //TODO 整理程式架構
        private IShopSerivce _shopSerivce;
        private void InitSerivce()
        {
            _shopSerivce = new ShopSerivce();
            MainDataCenter.Instance.SetIShopService(_shopSerivce);

        }
        #endregion



        #region ViewPage
        public MainProductPageView MainUserContent;
        public LoginView LoginUserContent;

        private UserControl _myUserControlInstance;
        public UserControl MyUserControlInstance
        {
            get { return _myUserControlInstance; }
            set
            {
                _myUserControlInstance = value;
                RaisePropertyChanged(nameof(MyUserControlInstance));
            }
        }


        #endregion



    }
}
