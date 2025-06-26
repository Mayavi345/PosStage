using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Utilities;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using Stage.Backstage.ProductManager.ViewModel;
using Stage.BLL.BLL;
using Utilities.Nlog;
using Stage.Data.Models.Implement_Model;
using Stage.BLL.BLL.Service;
using GalaSoft.MvvmLight.Messaging;

namespace Stage.Backstage
{
    public class AddProductViewModel : ObservableObject
    {
        #region Field
        private IProductService _productsService;
        private Action _refreshGridView;
        private IProductSettingManager _productSettingManager => MainDataCenter.Instance.ProductSettingManager;

        #endregion
        #region Constructor
        public AddProductViewModel(Action refreshGridView)
        {
            this._productsService = MainSystemService.Instance.ProductService;
            SelectedProductModel = new ProductSingleViewModel();
            _productSettingManager.LoadData();
            var CategoryList = _productSettingManager.CategoryList;
            SelectedProductModel.CategoryList = CategoryList;
            SelectedProductModel.SelectedItem = CategoryList[0];
            _refreshGridView = refreshGridView;
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
        private ObservableCollection<CategoryModel> _categoryTypeList;
        public ObservableCollection<CategoryModel> CategoryTypeList
        {
            get { return _categoryTypeList; }
            set
            {
                if (_categoryTypeList != value)
                {
                    _categoryTypeList = value;
                    RaisePropertyChanged(nameof(CategoryTypeList));
                }
            }
        }
        private RelayCommand<ProductModel> _addCommand;
        public RelayCommand<ProductModel> AddCommand
        {
            get
            {
                if (_addCommand == null)
                {
                    _addCommand = new RelayCommand<ProductModel>(AddCommandAction);
                }
                return _addCommand;
            }
            set { _addCommand = value; }
        }
        #endregion
        #region Private Method
        private void AddCommandAction(ProductModel model)
        {
            try
            {
                if (!SelectedProductModel.CheckIsValidate())
                {
                    MainSystemService.Instance.ShowMessageBox(TextResourceCenter.FiledValidateFail);
                    return;
                }
                else
                {
                    model.ProductId = _productsService.GetNewProductId();
                    int imageId = 0;
                    if (SelectedProductModel.AddImage(out imageId))
                    {
                        model.ImageId = imageId;
                    }
                    _productsService.Add(model);
                    _refreshGridView?.Invoke();
                    MainSystemService.Instance.ShowMessageBox(TextResourceCenter.AddSuccess);

                }
                SelectedProductModel.ClearData();
                Messenger.Default.Send(new CloseWindow(WindowType.Add), "CloseWindowMessageChannel");

            }
            catch (Exception e)
            {
                SelectedProductModel.ClearData();
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ShowMessageBox(e.Message);
            }
        }
        #endregion

    }
}
