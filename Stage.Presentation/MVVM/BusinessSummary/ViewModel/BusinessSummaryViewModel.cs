using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Stage.BLL.BLL;
using Stage.BLL.BLL.Service;
using System.Collections.Generic;
using System.Linq;
using PosStage.MVVM.Models;
using PosStage.MVVM.Models.Implement_Model;
using UIComponent.View;
using CodingNinja.Wpf.ObjectModel;
using System.Windows.Input;
using Utilities.Nlog;
using Utilities;
using System.ComponentModel;
using System.Windows.Data;
using Stage.Data.Models;

namespace PosStage.MVVM.ViewModel
{
    public class BusinessSummaryViewModel : ObservableObject
    {
        #region Field
        private readonly ICompletedOrdersService _completedOrdersService;
        private IFilter<IFilterableObject> _filterTextHelper;
        private OrderModel _currentOrderModel;
        #endregion

        #region Constructor
        public BusinessSummaryViewModel()
        {
            this._completedOrdersService = MainSystemService.Instance.CompletedOrdersService;
            NavBarViewModel = new NavBarViewModel();


            OrderModelList = new WpfObservableRangeCollection<OrderModel>();
            RefreshAllOrderList();
            SearchText = string.Empty;

            ProductModelList = new WpfObservableRangeCollection<ProductModel>();
            DataGridSelectionChangedCommand = new RelayCommand<object>(OnDataGridSelectionChanged);
            _filterTextHelper = new FilterTextHelper(SearchText);
            SortDataItems("Timestamp", ListSortDirection.Descending);
        }
        public void Init()
        {
            RefreshAllOrderList();
        }
        #endregion
        #region Properties
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
        private ICollectionView _orderModelCollection;
        public ICollectionView OrderModelCollection
        {
            get { return _orderModelCollection; }
            set
            {
                if (_orderModelCollection != value)
                {
                    _orderModelCollection = value;
                    RaisePropertyChanged(nameof(OrderModelCollection));

                }
            }
        }
        private WpfObservableRangeCollection<OrderModel> _orderModelList;
        /// <summary>
        /// 訂單列表
        /// </summary>
        public WpfObservableRangeCollection<OrderModel> OrderModelList
        {
            get { return _orderModelList; }
            set
            {
                if (_orderModelList != value)
                {
                    _orderModelList = value;
                    RaisePropertyChanged(nameof(OrderModelList));
                }
            }
        }
        private WpfObservableRangeCollection<ProductModel> _productModelList;
        /// <summary>
        /// 商品列表
        /// </summary>
        public WpfObservableRangeCollection<ProductModel> ProductModelList
        {
            get { return _productModelList; }
            set
            {
                if (_productModelList != value)
                {
                    _productModelList = value;
                    RaisePropertyChanged(nameof(ProductModelList));
                }
            }
        }
        // 訂單資訊
        private ProductModel _selectedProductModel;
        /// <summary>
        /// 當前選擇的商品
        /// </summary>
        public ProductModel SelectedProductModel
        {
            get { return _selectedProductModel; }
            set
            {
                if (_selectedProductModel != value)
                {
                    _selectedProductModel = value;
                    RaisePropertyChanged(nameof(SelectedProductModel));
                }
            }
        }
        private string _totalPrice;
        public string TotalPrice
        {
            get { return _totalPrice; }
            set
            {
                if (_totalPrice != value)
                {
                    _totalPrice = value;
                    RaisePropertyChanged(nameof(TotalPrice));
                }
            }
        }
        private string _productTotalAmount;
        public string ProductTotalAmount
        {
            get { return _productTotalAmount; }
            set
            {
                if (_productTotalAmount != value)
                {
                    _productTotalAmount = value;
                    RaisePropertyChanged(nameof(ProductTotalAmount));
                }
            }
        }
        private MemberModel _memberModel;
        public MemberModel MemberModel
        {
            get { return _memberModel; }
            set
            {
                if (_memberModel != value)
                {
                    _memberModel = value;
                    RaisePropertyChanged(nameof(MemberModel));
                }
            }
        }
        private DateRange _viewModelDate;
        public DateRange ViewModelDateList
        {
            get { return _viewModelDate; }
            set
            {
                if (_viewModelDate != value)
                {
                    _viewModelDate = value;
                    RaisePropertyChanged(nameof(ViewModelDateList));
                    DateChange(_viewModelDate);
                }
            }
        }
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;

                    SearchAction();
                    RaisePropertyChanged(nameof(SearchText));
                }
            }
        }
        #endregion
        #region Command
        private RelayCommand<OrderModel> _editSelectOrderProducCommandt;
        public RelayCommand<OrderModel> EditSelectOrderProductCommand
        {
            get
            {
                if (_editSelectOrderProducCommandt == null)
                {
                    _editSelectOrderProducCommandt = new RelayCommand<OrderModel>(EditSelectOrderProductCommandAction);
                }
                return _editSelectOrderProducCommandt;
            }
            set { _editSelectOrderProducCommandt = value; }
        }
        private RelayCommand _searchCommand;

        public RelayCommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                {
                    _searchCommand = new RelayCommand(SearchAction);
                }
                return _searchCommand;
            }
            set { _searchCommand = value; }
        }
        public ICommand DataGridSelectionChangedCommand { get; set; }


        private void OnDataGridSelectionChanged(object parameter)
        {
            if (parameter is System.Collections.IList selectedItems)
            {
                //拿到的是SelectedItemCollection，要轉換成OderMOdel
                var orderModel = selectedItems.Cast<OrderModel>().FirstOrDefault();
                if (orderModel == null)
                {
                    //在點選單個訂單的時候，會造成Null
                }
                else
                {
                    EditSelectOrderProductCommandAction(orderModel);
                }
            }
            {
                string message = "OnDataGridSelectionChanged OrderModel 型別錯誤";
                LogManagerSingleton.Instance.PrintLog(message, NLog.LogLevel.Error);
            }

        }
        #endregion
        #region Public Method
        public void SortDataItems(string propertyName, ListSortDirection direction)
        {
            if (_orderModelCollection != null)
            {
                _orderModelCollection.SortDescriptions.Clear();
                _orderModelCollection.SortDescriptions.Add(new SortDescription(propertyName, direction));
            }
        }
        #endregion
        #region Private Method   
        private void SearchAction()
        {
            if (!string.IsNullOrEmpty(_searchText))
            {
                _filterTextHelper.UpdateFilterText(_searchText);
                OrderModelCollection.Filter = _filterTextHelper.ContainsFilter;
            }
            else
            {
                RefreshAllOrderList();
            }
        }
        private void EditSelectOrderProductCommandAction(OrderModel model)
        {
            _currentOrderModel = model;
            RefreshProductModelList();
        }
        private void DateChange(DateRange viewModelDate)
        {
            OrderModelList = LoadOrderModelList(viewModelDate);
        }
        private void RefreshAllOrderList()
        {
            List<PB1001_CompletedOrders> tempList = _completedOrdersService.GetAllCompletedOrders();
            var orderModelList = MapOrderModel(tempList);
            OrderModelList = orderModelList;
            OrderModelCollection = CollectionViewSource.GetDefaultView(OrderModelList);
            OrderModelCollection.Filter = null;
        }
        private WpfObservableRangeCollection<OrderModel> MapOrderModel(List<PB1001_CompletedOrders> tempList)
        {
            OrderModelList.Clear();
            List<OrderModel> productModels = new List<OrderModel>();

            foreach (var item in tempList)
            {
                productModels.Add(new OrderModel()
                {
                    OrderNumber = item.OrderNumber,
                    Timestamp = item.Timestamp,
                    TotalAmount = item.TotalPrice,
                    EmployeeName = MainSystemService.Instance.EmployeeService.FindEmployeeName(item.OrderEmployeeId)
                });
            }
            OrderModelList.Clear();
            OrderModelList.AddRange(productModels);
            return OrderModelList;
        }

        private WpfObservableRangeCollection<OrderModel> LoadOrderModelList(DateRange viewModelDate)
        {
            List<PB1001_CompletedOrders> tempList = _completedOrdersService.GetOrderListByDateRange(viewModelDate.StartDate, viewModelDate.EndDate);
            return MapOrderModel(tempList);
        }

        private void RefreshProductModelList()
        {
            ProductModelList.Clear();
            //取得所有該菜單的CompletedOrdersDetails
            var response = _completedOrdersService.GetSelectOrderDetailProduct(_currentOrderModel.OrderNumber);
            var responseMemberInfo = MainSystemService.Instance.MemberSerivce.GetMemberOrderInfo(_currentOrderModel.OrderNumber);

            if (response.IsSuccess)
            {
                List<ProductModel> orderDetailList = response.Data;
                ProductTotalAmount = orderDetailList.Sum(x => x.Quantity).ToString();
                TotalPrice = orderDetailList.Sum(_x => _x.ToltalPrice).ToString();
                ProductModelList.AddRange(orderDetailList);

                if (responseMemberInfo.IsSuccess)
                {
                    MemberModel = responseMemberInfo.Data;
                }
                else
                {
                    //TODO 優化效能
                    MemberModel = new MemberModel();
                    RefreshAllOrderList();
                }
            }
            else
            {
                RefreshAllOrderList();
            }

        }
        #endregion

    }

}

