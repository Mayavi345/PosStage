using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.BLL;
using Stage.BLL.BLL;
using Stage.DAL;
using Stage.Presentation.Common;
using Stage.Presentation.MVVM.Product.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using PosStage.MVVM.Models.Implement_Model;
using Utilities.Nlog;
using PosStage.MVVM.Models;
using CodingNinja.Wpf.ObjectModel;
using Stage.Data.Models.Implement_Model;
using Stage.BLL.BLL.Service;
using UIComponent;

namespace PosStage.MVVM.ViewModel
{
    public class ProductViewModel : ObservableObject
    {
        #region Field
        IShopSerivce _shopService = MainDataCenter.Instance.ShopSerivce;

        private const int FakeLoadTime = 5;
        private const int InitProductQuantity = 1;
        private IProductService _productsService;
        /// <summary>
        /// 當前的產品列表
        /// </summary>
        private List<ProductModel> _currentProductModelList;
        /// <summary>
        /// 當前已經選擇Toggle按鈕
        /// </summary>
        private Dictionary<string, CategoriesToggleModel> _selectedCategoriesDic;
        private CategoriesToggleModel _allCategoriesToggle;
        /// <summary>
        /// 單個產品的ViewModel列表
        /// </summary>
        List<ProductSlotViewModel> _productSlotViewModel;

        #endregion
        #region Constructor
        public ProductViewModel()
        {
            try
            {
                this._productsService = MainSystemService.Instance.ProductService;
                _currentProductModelList = new List<ProductModel>();
                _selectedCategoriesDic = new Dictionary<string, CategoriesToggleModel>();
                Categories = new WpfObservableRangeCollection<CategoriesToggleModel>();
                ProductViewModelList = new WpfObservableRangeCollection<ProductSlotViewModel>();
                _productSlotViewModel = new List<ProductSlotViewModel>();
                _allCategoriesToggle = new CategoriesToggleModel()
                {
                    Title = "All",
                    IsChecked = true,
                };

            }
            catch (Exception e)
            {
                LogManagerSingleton.Instance.PrintLog(e.Message, NLog.LogLevel.Error);
                MainSystemService.Instance.ObserverUIMessageBox.NotifyObservers(e.Message);
            }
        }
        public void Init()
        {
            //載入全部的產品
            LoadAllProductData();
            //首次載入已經啟動的類別
            FirstLoadEnableCategories();
            //更新已啟用的產品
            UpdateAllEnableProducts();
            InitFirstLoadProductModelData();
        }

        #endregion
        #region Properties
        /// <summary>
        /// 產品列表
        /// </summary>
        private WpfObservableRangeCollection<ProductSlotViewModel> _productViewModelList;
        public WpfObservableRangeCollection<ProductSlotViewModel> ProductViewModelList
        {
            get { return _productViewModelList; }
            set
            {
                if (_productViewModelList != value)
                {
                    _productViewModelList = value;
                    RaisePropertyChanged(nameof(ProductViewModelList));
                }
            }
        }
        /// <summary>
        /// 產品類別
        /// </summary>
        private WpfObservableRangeCollection<CategoriesToggleModel> _categories;
        public WpfObservableRangeCollection<CategoriesToggleModel> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
                RaisePropertyChanged(nameof(Categories));
            }
        }


        private ObservableCollection<ProductSlotViewModel> _items;
        public ObservableCollection<ProductSlotViewModel> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                RaisePropertyChanged(nameof(Items));
            }
        }
        private string _selectedCategory;
        public string SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    RaisePropertyChanged(nameof(SelectedCategory));
                }
            }
        }
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                RaisePropertyChanged(nameof(IsLoading));
            }
        }
        #endregion
        #region Command
        private RelayCommand<CategoriesToggleModel> _filterCategoriesCommand;
        public RelayCommand<CategoriesToggleModel> FilterCategoriesCommand
        {
            get
            {
                if (_filterCategoriesCommand == null)
                {
                    _filterCategoriesCommand = new RelayCommand<CategoriesToggleModel>(FilterCategoriesCommandAction);
                }
                return _filterCategoriesCommand;
            }
            set { _filterCategoriesCommand = value; }
        }
        private RelayCommand<CategoriesToggleModel> _allCategoriesCommand;
        #endregion
        #region Public Method


        #endregion
        #region Private Method
        /// <summary>
        /// 設定產品初始資料
        /// </summary>
        private void InitFirstLoadProductModelData()
        {
            foreach (var item in _currentProductModelList)
            {
                item.Quantity = InitProductQuantity;
                item.UpdateTotalPrice();
            }

            RefrechProductViewModel(_currentProductModelList);
        }
        private void UpdateAllEnableProducts()
        {
            #region Loading Effect
            IsLoading = true;
            //用來做假載入效果
            TimerHelper timer = new TimerHelper(FakeLoadTime, () =>
            {
                IsLoading = false;
            });
            timer.Start();
            #endregion

            LoadAllProductData();
            RefrechProductViewModel(_currentProductModelList);
        }

        private void RefrechProductViewModel(List<ProductModel> tempList)
        {
            _productSlotViewModel.Clear();
            //去除未啟用項目
            var removeCount = tempList.RemoveAll(x => x.State == 0);
            foreach (var item in tempList)
            {
                _productSlotViewModel.Add(new ProductSlotViewModel(_shopService)
                {
                    Product = item,
                    Source = ImageSourceProcess.Instance.GetImage(item.ImageId),
                });
            }
            ProductViewModelList.Clear();
            ProductViewModelList.AddRange(_productSlotViewModel);
        }

        private void LoadAllProductData()
        {
            _currentProductModelList = _productsService.GetAllProducts(false);
        }

        private void FirstLoadEnableCategories()
        {
            _selectedCategoriesDic.Clear();

            _selectedCategoriesDic.Add(_allCategoriesToggle.Title, _allCategoriesToggle);
            List<CategoryModel> tempList = _productsService.GetCategories(true);

            foreach (var item in tempList)
            {
                _selectedCategoriesDic.Add(item.CategoryName, new CategoriesToggleModel
                {
                    Title = item.CategoryName,
                    IsChecked = false,
                });
            }

            Categories.Clear();
            //加入All按鍵
            // Categories.Insert(0, _allCategoriesToggle);
            Categories.AddRange(_selectedCategoriesDic.Values.ToList());
        }
        /// <summary>
        /// Toogle Button 點選事件，篩選類別按鈕行為
        /// </summary>
        /// <param name="button"></param>
        private void FilterCategoriesCommandAction(CategoriesToggleModel button)
        {
            //如果點的是"全部" /更新所有類別
            if (button == _allCategoriesToggle)
            {
                foreach (var item in _selectedCategoriesDic.Values)
                {
                    if (item != _allCategoriesToggle)
                        item.IsChecked = false;
                }
                _allCategoriesToggle.IsChecked = true;
                UpdateAllEnableProducts();
                Categories.Clear();
                Categories.AddRange(_selectedCategoriesDic.Values.ToList());
                return;
            }

            //點擊一般分類
            if (button.IsChecked)
            {
                //選擇新的分類
                RegisterSelectGroup(button);
            }
            else
            {
                //取消選擇時若沒有其他分類被選擇，則恢復顯示全部商品
                bool anyChecked = _selectedCategoriesDic.Values.Any(x => x != _allCategoriesToggle && x.IsChecked);
                if (!anyChecked)
                {
                    _allCategoriesToggle.IsChecked = true;
                    UpdateAllEnableProducts();
                    Categories.Clear();
                    Categories.AddRange(_selectedCategoriesDic.Values.ToList());
                    return;
                }
            }

            Categories.Clear();
            Categories.AddRange(_selectedCategoriesDic.Values.ToList());
            RefreshProdcutBySelectCateroy(_selectedCategoriesDic);
        }

        private void RefreshProdcutBySelectCateroy(Dictionary<string, CategoriesToggleModel> selectedCategories)
        {
            _currentProductModelList.Clear();
            //加入已經存在分類的產品
            foreach (var item in selectedCategories.Values.Where(x => x.IsChecked == true))
            {
                //查出該分類的產品，加入到顯示UI ProductViewModelList
                _currentProductModelList.AddRange(
                    _productsService.GetProductsByCategoryName(item.Title)
                 );
            }

            RefrechProductViewModel(_currentProductModelList);
        }


        private void RegisterSelectGroup(CategoriesToggleModel currentSelectCategory)
        {
            if (_selectedCategoriesDic.ContainsKey(currentSelectCategory.Title))
            {
                foreach (var item in _selectedCategoriesDic)
                {
                    item.Value.IsChecked = false;
                }
                var currentButton = _selectedCategoriesDic[currentSelectCategory.Title];
                currentButton.IsChecked = true;
            }
        }
        //private void UnRegisterSelectGroup(CategoriesToggleModel currentSelectCategoryName)
        //{
        //    _selectedCategoriesDic.Remove(currentSelectCategoryName.Title);
        //}
        #endregion

    }
}
