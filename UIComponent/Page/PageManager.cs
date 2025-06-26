using Stage.BLL;
using System;
using System.Windows.Controls;
using UIComponent.Page;
using Utilities;
using Utilities.Nlog;

namespace Stage.Presentation.Common
{
    public class PageManager : Singleton<PageManager>
    {
        private Action<UserControl> _switchUserControl;
        private IPageHelper _pageHelper;
        public void Init(Action<UserControl> switchUserControl, IPageHelper pageHelper)
        {
            _switchUserControl = switchUserControl;
            _pageHelper = pageHelper;
        }
        #region Page Swich

        public void SwitchToPage(EViewPage eViewPage)
        {

            PageObject currentPage = _pageHelper.GetPageObject(eViewPage);
            _switchUserControl.Invoke(currentPage.userControl);

            LogManagerSingleton.Instance.PrintLog($"currentPage:{currentPage.userControl}", NLog.LogLevel.Info);
        }

        #endregion
    }
}
