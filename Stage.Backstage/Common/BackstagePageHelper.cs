using Stage.Backstage.ViewModel;
using Stage.BLL;
using Stage.BLL.BLL;
using Stage.Presentation.Common;
using System;
using System.Collections.Generic;
using UIComponent.Page;
using UIComponent.View;
using Utilities.Nlog;

namespace Stage.Backstage
{
    public class BackstagePageHelper : IPageHelper
    {

        Dictionary<EViewPage, PageObject> PageObjectDic;
        public BackstagePageHelper()
        {
            PageObjectDic = new Dictionary<EViewPage, PageObject>();
            InitPageData();
        }

        private void InitPageData()
        {
            try
            {
                CreateLoginPage();
                CreateProductPage();
                CrateEmployeePage();
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

        private void CrateEmployeePage()
        {
            EmployeeManagerViewModel employeeManagerViewModel = new EmployeeManagerViewModel();
            PageObject employeeManagerView =
                new PageObject(new EmployeeManagerView(), employeeManagerViewModel, employeeManagerViewModel.RefreshGridCallBack);
            PageObjectDic.Add(EViewPage.EmployeeManagerView, employeeManagerView);
        }

        private void CreateProductPage()
        {
            ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel();
            PageObject productManagerView =
                new PageObject(new ProductManagerView(), productManagerViewModel, productManagerViewModel.Init);
            PageObjectDic.Add(EViewPage.ProductManagerView, productManagerView);
        }

        public PageObject GetPageObject(EViewPage eViewPage)
        {
            PageObjectDic[eViewPage].InitAction?.Invoke();
            return PageObjectDic[eViewPage];
        }
    }
}