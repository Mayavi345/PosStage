using GalaSoft.MvvmLight;
using System;
using System.Windows.Controls;
using Utilities.Nlog;

namespace UIComponent.Page
{
    public class PageObject
    {
        public UserControl userControl => _userControl;
        private UserControl _userControl;

        public ObservableObject observableObject => _observableObject;
        private ObservableObject _observableObject;
        public Action InitAction => _initAction;
        public Action _initAction;

        public PageObject(UserControl userControl, ObservableObject observableObject, Action InitAction)
        {
            LogManagerSingleton.Instance.PrintLog(userControl.Name, NLog.LogLevel.Info);
            _userControl = userControl;
            _observableObject = observableObject;
            _initAction = InitAction;
            PageObjectBindingModel();
        }
        public void PageObjectBindingModel()
        {
            userControl.DataContext = observableObject;
        }

    }
}
