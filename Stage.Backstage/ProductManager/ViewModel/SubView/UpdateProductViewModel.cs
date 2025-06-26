using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using PosStage.MVVM.Models.Implement_Model;
using Stage.BLL.BLL;
using Utilities.Nlog;
using Utilities;
using Stage.BLL.BLL.Service;
using GalaSoft.MvvmLight.Messaging;

namespace Stage.Backstage.ProductManager.ViewModel
{
    public class UpdateProductViewModel : ObservableObject
    {
        #region Field
        private IProductService _productsService;
        private Action _refreshGridView;
        private IProductSettingManager _productSettingManager => MainDataCenter.Instance.ProductSettingManager;

        #endregion
        #region Constructor
        public UpdateProductViewModel(Action refreshGridView)
        {
            _refreshGridView = refreshGridView;
            SelectedProductModel = new ProductSingleViewModel();
            this._productsService = MainSystemService.Instance.ProductService;
            //  CategoriesComboBoxViewModel = new CategoriesComboBoxViewModel(categoryTypeList);

        }
        #endregion
        #region Properties
        private ProductSingleViewModel _currentSelectProductModel;
        public ProductSingleViewModel SelectedProductModel
        {
            get { return _currentSelectProductModel; }
            set
            {
                if (_currentSelectProductModel != value)
                {
                    _currentSelectProductModel = value;
                    RaisePropertyChanged(nameof(SelectedProductModel));
                }
            }
        }
        private bool _isUpdateEditEnable;
        public bool IsUpdateEditEnable
        {
            get { return _isUpdateEditEnable; }
            set
            {
                if (_isUpdateEditEnable != value)
                {
                    _isUpdateEditEnable = value;
                    RaisePropertyChanged(nameof(IsUpdateEditEnable));
                }
            }
        }
        #endregion
        #region Command
        private RelayCommand<ProductModel> _updateCommand;
        public RelayCommand<ProductModel> UpdateCommand
        {
            get
            {
                if (_updateCommand == null)
                {
                    _updateCommand = new RelayCommand<ProductModel>(UpdateCommandAction);
                }
                return _updateCommand;
            }
            set { _updateCommand = value; }
        }
        private RelayCommand _updateCancelCommand;
        public RelayCommand UpdateCancelCommand
        {
            get
            {
                if (_updateCancelCommand == null)
                {
                    _updateCancelCommand = new RelayCommand(UpdateCancelCommandAction);
                }
                return _updateCancelCommand;
            }
            set { _updateCancelCommand = value; }
        }
        #endregion
        #region Public Method

        #endregion
        #region Private Method
        private void UpdateCommandAction(ProductModel model)
        {
            try
            {
                if (!SelectedProductModel.CheckIsValidate())
                {
                    MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(TextResourceCenter.FiledValidateFail);
                }
                else
                {
                    IsUpdateEditEnable = false;
                    int imageId = 0;
                    if (SelectedProductModel.AddImage(out imageId))
                    {
                        model.ImageId = imageId;
                    }
                    _productsService.Update(model);
                    _refreshGridView?.Invoke();
                    Messenger.Default.Send(new CloseWindow(WindowType.Update), "CloseWindowMessageChannel");
                }
                SelectedProductModel.ClearData();
            }
            catch (Exception e)
            {
                SelectedProductModel.ClearData();
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }
        private void UpdateCancelCommandAction()
        {
            IsUpdateEditEnable = false;
            SelectedProductModel.ClearData();

        }
        #endregion
        //private CategoriesComboBoxViewModel _categoriesComboBoxViewModel;
        //public CategoriesComboBoxViewModel CategoriesComboBoxViewModel
        //{
        //    get { return _categoriesComboBoxViewModel; }
        //    set
        //    {
        //        if (_categoriesComboBoxViewModel != value)
        //        {
        //            _categoriesComboBoxViewModel = value;
        //            RaisePropertyChanged(nameof(CategoriesComboBoxViewModel));
        //        }
        //    }
        //}
    }
}
