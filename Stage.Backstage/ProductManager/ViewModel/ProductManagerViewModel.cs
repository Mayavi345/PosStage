using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.Backstage.Common;
using Stage.Backstage.ProductManager.ViewModel;
using Stage.BLL.BLL;
using System.Collections.ObjectModel;
using System.Windows;
using PosStage.MVVM.Models.Implement_Model;
using Utilities;
using Stage.BLL.BLL.Service;
using System.Windows.Input;
using System;
using UIComponent.Controls;
using Stage.Backstage.ProductManager.View;
using System.Collections.Generic;
using CodingNinja.Wpf.ObjectModel;
using static System.Windows.Forms.AxHost;
using UIComponent;
using GalaSoft.MvvmLight.Messaging;

namespace Stage.Backstage
{
    public class ProductManagerViewModel : ObservableObject
    {
        private IProductService _productsService;
        private UpdateProductWindowViewModel _updateProductWindowViewModel;
        private IProductSettingManager _productSettingManager => MainDataCenter.Instance.ProductSettingManager;

        public ProductManagerViewModel()
        {
            this._productsService = MainSystemService.Instance.ProductService;
            NavBarViewModel = new NavBarViewModel();
            MainDataCenter.Instance.RefreshProductView.RegisterObserver(new RefreshProductViewObserver(Init));
            CategoryViewModel = new CategoryViewModel();
            ProductDGList = new WpfObservableRangeCollection<ProductDataGridSingleViewModel>();
            _updateProductWindowViewModel = new UpdateProductWindowViewModel(RefreshProductGrid);
            Messenger.Default.Register<CloseWindow>(this, "CloseWindowMessageChannel", CloseWindowMessage);
            CategoryViewModel.Init();
            OpenAddPageCommand = new RelayCommand(OpenAddPageCommandAction);
            Init();
        }
        #region Properties
        private CategoryViewModel _categoryViewModel;
        public CategoryViewModel CategoryViewModel
        {
            get { return _categoryViewModel; }
            set
            {
                if (_categoryViewModel != value)
                {
                    _categoryViewModel = value;
                    RaisePropertyChanged(nameof(CategoryViewModel));
                }
            }
        }
        private WpfObservableRangeCollection<ProductDataGridSingleViewModel> _productDGList;
        public WpfObservableRangeCollection<ProductDataGridSingleViewModel> ProductDGList
        {
            get { return _productDGList; }
            set
            {
                if (_productDGList != value)
                {
                    _productDGList = value;
                    RaisePropertyChanged(nameof(ProductDGList));
                }
            }
        }
        private NavBarViewModel _navBarViewModel;
        public NavBarViewModel NavBarViewModel
        {
            get { return _navBarViewModel; }
            set
            {
                if (_navBarViewModel != value)
                {
                    _navBarViewModel = value;
                    RaisePropertyChanged(nameof(NavBarViewModel));
                }
            }
        }

        #endregion
        public void Init()
        {
            RefreshProductGrid();
        }
        public void RefreshProductGrid()
        {
            ProductDGList = RefreshProduct();
        }
        #region Internal
        private WpfObservableRangeCollection<ProductDataGridSingleViewModel> RefreshProduct()
        {
            //TODO 優化效能 new
            List<ProductModel> _currentProductModelList = _productsService.GetAllProducts(true);
            ProductDGList.Clear();
            foreach (var item in _currentProductModelList)
            {
                ProductDataGridSingleViewModel productDataGridSingleViewModel =
                    new ProductDataGridSingleViewModel(item);
                ProductDGList.Add(productDataGridSingleViewModel);
            }
            return ProductDGList;
        }
        #endregion
        #region Command

        private RelayCommand<ProductDataGridSingleViewModel> _editCommand;

        public RelayCommand<ProductDataGridSingleViewModel> EditCommand
        {
            get
            {
                if (_editCommand == null)
                {
                    _editCommand = new RelayCommand<ProductDataGridSingleViewModel>(EditCommandAction);
                }
                return _editCommand;
            }
            set { _editCommand = value; }
        }

        private RelayCommand<ProductDataGridSingleViewModel> _deleteCommand;

        public RelayCommand<ProductDataGridSingleViewModel> DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                {
                    _deleteCommand = new RelayCommand<ProductDataGridSingleViewModel>(DeleteCommandAction);
                }
                return _deleteCommand;
            }
            set { _deleteCommand = value; }
        }
        public ICommand OpenAddPageCommand { get; set; }

        #endregion

        private void DeleteCommandAction(ProductDataGridSingleViewModel model)
        {
            UIMessageBoxOKCancel.Instance.ShowDialog(() =>
            {
                _productsService.DeleteProductById(model.ProductModel);
                RefreshProductGrid();
            }, () => { });
        }


        private void EditCommandAction(ProductDataGridSingleViewModel model)
        {
            var updateProductViewModel = _updateProductWindowViewModel.UpdateProductViewModel;
            SingletonWindowManager.ShowSingletonWindow<UpdateProductWindowView>(_updateProductWindowViewModel);
            _productSettingManager.SetSelectProductModel(model.ProductModel);
            updateProductViewModel.SelectedProductModel.InitSingleViewData(() => { });
            updateProductViewModel.IsUpdateEditEnable = true;

        }
        private void OpenAddPageCommandAction()
        {
            SingletonWindowManager.ShowSingletonWindow<AddProductWindowView>(new AddProductWindowsViewModel(RefreshProductGrid));

        }
        private static void CloseAddWindow()
        {
            Window currentWindow = SingletonWindowManager.GetWindow<AddProductWindowView>();
            currentWindow.Close();
        }

        private static void CloseUpdateWindow()
        {
            Window currentWindow = SingletonWindowManager.GetWindow<UpdateProductWindowView>();
            currentWindow.Close();
        }
        private void CloseWindowMessage(CloseWindow window)
        {
            switch (window.windowType)
            {
                case WindowType.Update:
                    CloseUpdateWindow();
                    break;
                case WindowType.Add:
                    CloseAddWindow();
                    break;
            }
        }

    }
    public class CloseWindow
    {
        public WindowType windowType;

        public CloseWindow(WindowType windowType)
        {
            this.windowType = windowType;
        }
    }
    public enum WindowType
    {
        Update,
        Add
    }
}
