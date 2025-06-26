using Stage.Presentation.Common;
using Stage.Presentation.MVVM.MainPage.ViewModel;
using Stage.Presentation.MVVM.MemberManager;
using System.Collections.Generic;
using UIComponent.Page;
using PosStage.MVVM.View;
using PosStage.MVVM.ViewModel;
using System;
using Stage.BLL.BLL;
using Utilities.Observer;
using System.Windows;
using Utilities.Nlog;
using UIComponent.View;

namespace Stage.BLL
{
    public class PageHelper : IPageHelper
    {
        Dictionary<EViewPage, PageObject> PageObjectDic;
        private MessageSubject _observerUIMessageBox => MainSystemService.Instance.ObserverUIMessageBox;

        public PageHelper()
        {
            PageObjectDic = new Dictionary<EViewPage, PageObject>();
            InitPageData();
        }

        private void InitPageData()
        {
            try
            {
                LogManagerSingleton.Instance.PrintLog("InitPageData Start", NLog.LogLevel.Info);

                CreateLoginPage();

                var mainProductPageViewModel = new MainProductPageViewModel();
                PageObject mainPageViewPageObject = new PageObject(new MainProductPageView(),
                   mainProductPageViewModel, mainProductPageViewModel.Init);
                PageObjectDic.Add(EViewPage.MainPageView, mainPageViewPageObject);

                var businessSummaryViewModel = new BusinessSummaryViewModel();
                PageObject businessSummaryViewPageObject = new PageObject(new BusinessSummaryView(),
                  businessSummaryViewModel, businessSummaryViewModel.Init);
                PageObjectDic.Add(EViewPage.BusinessSummaryView, businessSummaryViewPageObject);
                AddMemberPage();

                LogManagerSingleton.Instance.PrintLog("InitPageData End", NLog.LogLevel.Info);

            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }

        private void AddMemberPage()
        {
            try
            {
                var memberManagerViewModel = new MemberManagerViewModel();
                PageObject memberManagerViewPageObject = new PageObject(new MemberManagerView(), memberManagerViewModel, memberManagerViewModel.LoadMemberDGList);
                PageObjectDic.Add(EViewPage.MemberManagerView, memberManagerViewPageObject);
            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }
        private void CreateLoginPage()
        {
            LoginViewModel viewModel = new LoginViewModel();
            PageObject view =
                new PageObject(new LoginView(), viewModel, viewModel.Init);
            PageObjectDic.Add(EViewPage.LoginView, view);
        }
        public PageObject GetPageObject(EViewPage eViewPage)
        {
            PageObjectDic[eViewPage].InitAction?.Invoke();
            return PageObjectDic[eViewPage];
        }
    }
}
